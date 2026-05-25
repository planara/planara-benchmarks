using HotChocolate;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Responses;

/// <summary>
/// Детальный отчет запуска бенчмарка
/// </summary>
[GraphQLDescription("Детальный отчет запуска бенчмарка")]
public sealed class BenchmarkRunResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public double DurationMs { get; set; }

    public BenchmarkRunStatus Status { get; set; }

    public string? UserAgent { get; set; }

    public double? DevicePixelRatio { get; set; }

    public IReadOnlyList<BenchmarkTestResultResponse> Tests { get; set; } = [];

    public BenchmarkRunResponse(BenchmarkRun run)
    {
        Id = run.Id;
        CreatedAt = run.CreatedAt;
        CompletedAt = run.CompletedAt;
        DurationMs = run.DurationMs;
        Status = run.Status;
        UserAgent = run.UserAgent;
        DevicePixelRatio = run.DevicePixelRatio;
        Tests = run.Tests
            .Select(x => new BenchmarkTestResultResponse(x))
            .ToArray();
    }
}