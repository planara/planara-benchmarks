using HotChocolate;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Responses;

/// <summary>
/// Результат отдельного бенчмарк-теста
/// </summary>
[GraphQLDescription("Результат отдельного бенчмарк-теста")]
public sealed class BenchmarkTestResultResponse
{
    public Guid Id { get; set; }

    public BenchmarkTestType Type { get; set; }

    public BenchmarkTestStatus Status { get; set; }

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

    public BenchmarkMetricsHistory History { get; set; }

    public BenchmarkTestResultResponse(BenchmarkTestResult result)
    {
        Id = result.Id;
        Type = result.Type;
        Status = result.Status;
        ErrorMessage = result.ErrorMessage;
        DurationMs = result.DurationMs;
        Frames = result.Frames;
        AverageFps = result.AverageFps;
        MinFps = result.MinFps;
        AverageFrameTime = result.AverageFrameTime;
        MaxFrameTime = result.MaxFrameTime;
        ObjectsCount = result.ObjectsCount;
        DrawCalls = result.DrawCalls;
        Triangles = result.Triangles;
        Geometries = result.Geometries;
        Textures = result.Textures;
        MemoryUsedMb = result.MemoryUsedMb;
        History = result.History;
    }
}