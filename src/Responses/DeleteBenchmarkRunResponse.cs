using HotChocolate;

namespace Planara.Benchmarks.Responses;

/// <summary>
/// Результат удаления запуска бенчмарка
/// </summary>
[GraphQLDescription("Результат удаления запуска бенчмарка")]
public sealed class DeleteBenchmarkRunResponse
{
    public bool Success { get; set; }
}