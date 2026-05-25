using HotChocolate;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Requests;

/// <summary>
/// Запрос на сохранение результата отдельного бенчмарк-теста
/// </summary>
[GraphQLDescription("Запрос на сохранение результата отдельного бенчмарк-теста")]
public sealed class SaveBenchmarkTestResultRequest
{
    public BenchmarkTestType Type { get; set; }

    public BenchmarkTestStatus Status { get; set; } = BenchmarkTestStatus.Success;

    public string? ErrorMessage { get; set; }

    public double DurationMs { get; set; }

    public int Frames { get; set; }

    public double AverageFps { get; set; }

    public double MinFps { get; set; }

    public double AverageFrameTime { get; set; }

    public double MaxFrameTime { get; set; }

    public int ObjectsCount { get; set; }

    public int DrawCalls { get; set; }

    public int Triangles { get; set; }

    public int Geometries { get; set; }

    public int Textures { get; set; }

    public double? MemoryUsedMb { get; set; }

    public BenchmarkMetricsHistory History { get; set; } = new();
}