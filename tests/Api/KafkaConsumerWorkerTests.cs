using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Data.Enums;
using Planara.Benchmarks.Tests.Fakes;
using Planara.Benchmarks.Workers;
using Planara.Common.Kafka;

namespace Planara.Benchmarks.Tests.Api;

public class KafkaConsumerWorkerTests : BaseApiTest
{
    public KafkaConsumerWorkerTests(ApiTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task UserDeletedConsumer_ConsumeOnce_ExistingBenchmarkData_DeletesRunsAndTests_AndCommits()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        using var scope = Factory.Services.CreateScope();

        var fake = scope.ServiceProvider
            .GetRequiredService<FakeKafkaConsumer<UserDeletedMessage>>();

        fake.Reset();

        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();

        var userRun = CreateRun(userId, "User run");
        var anotherUserRun = CreateRun(anotherUserId, "Another user run");

        Context.BenchmarkRuns.AddRange(userRun, anotherUserRun);

        Context.BenchmarkTestResults.AddRange(
            CreateTest(userRun.Id, BenchmarkTestType.Light),
            CreateTest(userRun.Id, BenchmarkTestType.Heavy),
            CreateTest(anotherUserRun.Id, BenchmarkTestType.Medium));

        await Context.SaveChangesAsync();

        var worker = scope.ServiceProvider
            .GetRequiredService<UserDeletedKafkaConsumerWorker>();

        fake.Results.Enqueue(FakeKafkaConsumer<UserDeletedMessage>.CreateResult(
            new UserDeletedMessage
            {
                UserId = userId
            }));

        await worker.ConsumeOnce(CancellationToken.None);

        fake.ConsumedTopicKeys.Should().ContainSingle()
            .Which.Should().Be(KafkaTopicKeys.UserDeleted);

        fake.Committed.Should().HaveCount(1);

        Context.ChangeTracker.Clear();

        var deletedUserRunsCount = await Context.BenchmarkRuns
            .AsNoTracking()
            .CountAsync(x => x.UserId == userId);

        deletedUserRunsCount.Should().Be(0);

        var deletedUserTestsCount = await Context.BenchmarkTestResults
            .AsNoTracking()
            .CountAsync(x => x.RunId == userRun.Id);

        deletedUserTestsCount.Should().Be(0);

        var remainingRun = await Context.BenchmarkRuns
            .AsNoTracking()
            .SingleAsync(x => x.UserId == anotherUserId);

        remainingRun.Name.Should().Be("Another user run");

        var remainingTestsCount = await Context.BenchmarkTestResults
            .AsNoTracking()
            .CountAsync(x => x.RunId == anotherUserRun.Id);

        remainingTestsCount.Should().Be(1);
    }

    [Fact]
    public async Task UserDeletedConsumer_ConsumeOnce_WhenUserHasNoBenchmarkData_Commits()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        using var scope = Factory.Services.CreateScope();

        var fake = scope.ServiceProvider
            .GetRequiredService<FakeKafkaConsumer<UserDeletedMessage>>();

        fake.Reset();

        var worker = scope.ServiceProvider
            .GetRequiredService<UserDeletedKafkaConsumerWorker>();

        fake.Results.Enqueue(FakeKafkaConsumer<UserDeletedMessage>.CreateResult(
            new UserDeletedMessage
            {
                UserId = Guid.NewGuid()
            }));

        await worker.ConsumeOnce(CancellationToken.None);

        fake.ConsumedTopicKeys.Should().ContainSingle()
            .Which.Should().Be(KafkaTopicKeys.UserDeleted);

        fake.Committed.Should().HaveCount(1);

        var runsCount = await Context.BenchmarkRuns.CountAsync();
        var testsCount = await Context.BenchmarkTestResults.CountAsync();

        runsCount.Should().Be(0);
        testsCount.Should().Be(0);
    }

    [Fact]
    public async Task UserDeletedConsumer_ConsumeOnce_NullResult_DoesNotCommit()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        using var scope = Factory.Services.CreateScope();

        var fake = scope.ServiceProvider
            .GetRequiredService<FakeKafkaConsumer<UserDeletedMessage>>();

        fake.Reset();

        var worker = scope.ServiceProvider
            .GetRequiredService<UserDeletedKafkaConsumerWorker>();

        await worker.ConsumeOnce(CancellationToken.None);

        fake.ConsumedTopicKeys.Should().ContainSingle()
            .Which.Should().Be(KafkaTopicKeys.UserDeleted);

        fake.Committed.Should().BeEmpty();

        var runsCount = await Context.BenchmarkRuns.CountAsync();

        runsCount.Should().Be(0);
    }

    [Fact]
    public async Task UserDeletedConsumer_ConsumeOnce_NullMessage_DoesNotCommit()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        using var scope = Factory.Services.CreateScope();

        var fake = scope.ServiceProvider
            .GetRequiredService<FakeKafkaConsumer<UserDeletedMessage>>();

        fake.Reset();

        var worker = scope.ServiceProvider
            .GetRequiredService<UserDeletedKafkaConsumerWorker>();

        fake.Results.Enqueue(
            FakeKafkaConsumer<UserDeletedMessage>.CreateNullMessageResult());

        await worker.ConsumeOnce(CancellationToken.None);

        fake.Committed.Should().BeEmpty();

        var runsCount = await Context.BenchmarkRuns.CountAsync();

        runsCount.Should().Be(0);
    }

    [Fact]
    public async Task UserDeletedConsumer_ConsumeOnce_NullInnerMessage_DoesNotCommit()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        using var scope = Factory.Services.CreateScope();

        var fake = scope.ServiceProvider
            .GetRequiredService<FakeKafkaConsumer<UserDeletedMessage>>();

        fake.Reset();

        var worker = scope.ServiceProvider
            .GetRequiredService<UserDeletedKafkaConsumerWorker>();

        fake.Results.Enqueue(
            FakeKafkaConsumer<UserDeletedMessage>.CreateNullInnerMessageResult());

        await worker.ConsumeOnce(CancellationToken.None);

        fake.Committed.Should().BeEmpty();

        var runsCount = await Context.BenchmarkRuns.CountAsync();

        runsCount.Should().Be(0);
    }

    private static BenchmarkRun CreateRun(Guid userId, string name)
    {
        return new BenchmarkRun
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            CompletedAt = DateTime.UtcNow,
            DurationMs = 1000,
            Status = BenchmarkRunStatus.Completed,
            UserAgent = "tests",
            DevicePixelRatio = 1
        };
    }

    private static BenchmarkTestResult CreateTest(
        Guid runId,
        BenchmarkTestType type)
    {
        return new BenchmarkTestResult
        {
            Id = Guid.NewGuid(),
            RunId = runId,
            Type = type,
            Status = BenchmarkTestStatus.Success,
            ErrorMessage = null,
            DurationMs = 100,
            Frames = 60,
            AverageFps = 60,
            MinFps = 55,
            AverageFrameTime = 16,
            MaxFrameTime = 20,
            ObjectsCount = 10,
            DrawCalls = 5,
            Triangles = 1000,
            Geometries = 3,
            Textures = 2,
            MemoryUsedMb = 128,
            History = new BenchmarkMetricsHistory
            {
                TimeMs = [0, 100],
                AverageFps = [60, 60],
                MinFps = [55, 56],
                AverageFrameTime = [16, 16],
                MaxFrameTime = [20, 19],
                MemoryUsedMb = [128, 129],
                DrawCalls = [5, 5],
                Triangles = [1000, 1000],
                ObjectsCount = [10, 10]
            }
        };
    }
}