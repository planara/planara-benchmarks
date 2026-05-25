using HotChocolate;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Responses;

/// <summary>
/// Краткая информация о запуске бенчмарка
/// </summary>
[GraphQLDescription("Краткая информация о запуске бенчмарка")]
public sealed class BenchmarkRunListItemResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public double DurationMs { get; set; }

    public BenchmarkRunStatus Status { get; set; }

    public int TestsCount { get; set; }

    public string? UserAgent { get; set; }

    public double? DevicePixelRatio { get; set; }
}