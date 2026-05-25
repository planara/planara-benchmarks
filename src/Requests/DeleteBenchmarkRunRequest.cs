using HotChocolate;

namespace Planara.Benchmarks.Requests;

/// <summary>
/// Запрос на удаление запуска бенчмарка
/// </summary>
[GraphQLDescription("Запрос на удаление запуска бенчмарка")]
public sealed class DeleteBenchmarkRunRequest
{
    /// <summary>
    /// ID запуска
    /// </summary>
    [GraphQLDescription("ID запуска")]
    public Guid RunId { get; set; }
}