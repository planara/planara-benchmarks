using HotChocolate;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Requests;

/// <summary>
/// Запрос на сохранение результата запуска бенчмарка
/// </summary>
[GraphQLDescription("Запрос на сохранение результата запуска бенчмарка")]
public sealed class SaveBenchmarkRunRequest
{
    /// <summary>
    /// Время завершения запуска
    /// </summary>
    [GraphQLDescription("Время завершения запуска")]
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Общая длительность запуска в миллисекундах
    /// </summary>
    [GraphQLDescription("Общая длительность запуска в миллисекундах")]
    public double DurationMs { get; set; }

    /// <summary>
    /// Статус запуска
    /// </summary>
    [GraphQLDescription("Статус запуска")]
    public BenchmarkRunStatus Status { get; set; } = BenchmarkRunStatus.Completed;

    /// <summary>
    /// User-Agent браузера
    /// </summary>
    [GraphQLDescription("User-Agent браузера")]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Device pixel ratio
    /// </summary>
    [GraphQLDescription("Device pixel ratio")]
    public double? DevicePixelRatio { get; set; }

    /// <summary>
    /// Результаты тестов внутри запуска
    /// </summary>
    [GraphQLDescription("Результаты тестов внутри запуска")]
    public required IReadOnlyList<SaveBenchmarkTestResultRequest> Tests { get; set; }
}