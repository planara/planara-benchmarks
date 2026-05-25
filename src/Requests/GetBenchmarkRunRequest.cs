using HotChocolate;

namespace Planara.Benchmarks.Requests;

/// <summary>
/// Запрос на получение запуска бенчмарка
/// </summary>
[GraphQLDescription("Запрос на получение запуска бенчмарка")]
public sealed class GetBenchmarkRunRequest
{
    /// <summary>
    /// ID запуска
    /// </summary>
    [GraphQLDescription("ID запуска")]
    public Guid RunId { get; set; }
}