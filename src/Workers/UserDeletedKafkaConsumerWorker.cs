using Microsoft.EntityFrameworkCore;
using Planara.Benchmarks.Data;
using Planara.Common.Kafka;
using Planara.Kafka.Interfaces;

namespace Planara.Benchmarks.Workers;

public class UserDeletedKafkaConsumerWorker(
    ILogger<UserDeletedKafkaConsumerWorker> logger,
    IKafkaConsumer<UserDeletedMessage> consumer,
    IServiceScopeFactory scopeFactory)
    : KafkaConsumerWorkerBase<UserDeletedMessage>(logger, consumer, scopeFactory)
{
    protected override string TopicKey => KafkaTopicKeys.UserDeleted;

    protected override async Task HandleMessage(
        UserDeletedMessage message,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Kafka: user deleted message received: userId - {UserId}", message.UserId);

        var dataContext = serviceProvider.GetRequiredService<DataContext>();

        var userRunIds = dataContext.BenchmarkRuns
            .Where(x => x.UserId == message.UserId)
            .Select(x => x.Id);

        var deletedTestsCount = await dataContext.BenchmarkTestResults
            .Where(x => userRunIds.Contains(x.RunId))
            .ExecuteDeleteAsync(cancellationToken);

        var deletedRunsCount = await dataContext.BenchmarkRuns
            .Where(x => x.UserId == message.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        logger.LogInformation(
            "Benchmark data deletion completed for userId={UserId}. Deleted runs: {RunsCount}, deleted tests: {TestsCount}",
            message.UserId,
            deletedRunsCount,
            deletedTestsCount);
    }
}