using FluentAssertions;
using Planara.Benchmarks.Data.Enums;
using Planara.Benchmarks.Requests;
using Planara.Benchmarks.Validators;

namespace Planara.Benchmarks.Tests.Unit;

public class ValidatorsTests
{
    [Fact]
    public void GetBenchmarkRun_EmptyRunId_Fails()
    {
        var validator = new GetBenchmarkRunRequestValidator();

        var request = new GetBenchmarkRunRequest
        {
            RunId = Guid.Empty
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void DeleteBenchmarkRun_EmptyRunId_Fails()
    {
        var validator = new DeleteBenchmarkRunRequestValidator();

        var request = new DeleteBenchmarkRunRequest
        {
            RunId = Guid.Empty
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SaveBenchmarkRun_ValidRequest_Succeeds()
    {
        var validator = new SaveBenchmarkRunRequestValidator();

        var request = new SaveBenchmarkRunRequest
        {
            DurationMs = 1000,
            Status = BenchmarkRunStatus.Completed,
            UserAgent = "Tests",
            DevicePixelRatio = 2,
            Tests =
            [
                CreateValidTest()
            ]
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SaveBenchmarkRun_NegativeDuration_Fails()
    {
        var validator = new SaveBenchmarkRunRequestValidator();

        var request = new SaveBenchmarkRunRequest
        {
            DurationMs = -1,
            Status = BenchmarkRunStatus.Completed,
            Tests =
            [
                CreateValidTest()
            ]
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SaveBenchmarkRun_EmptyTests_Fails()
    {
        var validator = new SaveBenchmarkRunRequestValidator();

        var request = new SaveBenchmarkRunRequest
        {
            DurationMs = 1000,
            Status = BenchmarkRunStatus.Completed,
            Tests = []
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SaveBenchmarkRun_LongUserAgent_Fails()
    {
        var validator = new SaveBenchmarkRunRequestValidator();

        var request = new SaveBenchmarkRunRequest
        {
            DurationMs = 1000,
            Status = BenchmarkRunStatus.Completed,
            UserAgent = new string('a', 513),
            Tests =
            [
                CreateValidTest()
            ]
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-1, 0, 60, 55, 16.6, 20, 10, 5, 1000, 3, 2, 128)]
    [InlineData(1000, -1, 60, 55, 16.6, 20, 10, 5, 1000, 3, 2, 128)]
    [InlineData(1000, 60, -1, 55, 16.6, 20, 10, 5, 1000, 3, 2, 128)]
    [InlineData(1000, 60, 60, -1, 16.6, 20, 10, 5, 1000, 3, 2, 128)]
    [InlineData(1000, 60, 60, 55, -1, 20, 10, 5, 1000, 3, 2, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, -1, 10, 5, 1000, 3, 2, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, 20, -1, 5, 1000, 3, 2, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, 20, 10, -1, 1000, 3, 2, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, 20, 10, 5, -1, 3, 2, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, 20, 10, 5, 1000, -1, 2, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, 20, 10, 5, 1000, 3, -1, 128)]
    [InlineData(1000, 60, 60, 55, 16.6, 20, 10, 5, 1000, 3, 2, -1)]
    public void SaveBenchmarkTestResult_NegativeMetrics_Fails(
        double durationMs,
        int frames,
        double averageFps,
        double minFps,
        double averageFrameTime,
        double maxFrameTime,
        int objectsCount,
        int drawCalls,
        int triangles,
        int geometries,
        int textures,
        double memoryUsedMb)
    {
        var validator = new SaveBenchmarkTestResultRequestValidator();

        var request = new SaveBenchmarkTestResultRequest
        {
            Type = BenchmarkTestType.Light,
            Status = BenchmarkTestStatus.Success,
            DurationMs = durationMs,
            Frames = frames,
            AverageFps = averageFps,
            MinFps = minFps,
            AverageFrameTime = averageFrameTime,
            MaxFrameTime = maxFrameTime,
            ObjectsCount = objectsCount,
            DrawCalls = drawCalls,
            Triangles = triangles,
            Geometries = geometries,
            Textures = textures,
            MemoryUsedMb = memoryUsedMb
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SaveBenchmarkTestResult_NullMemory_Succeeds()
    {
        var validator = new SaveBenchmarkTestResultRequestValidator();

        var request = CreateValidTest();
        request.MemoryUsedMb = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    private static SaveBenchmarkTestResultRequest CreateValidTest() => new()
    {
        Type = BenchmarkTestType.Light,
        Status = BenchmarkTestStatus.Success,
        DurationMs = 1000,
        Frames = 60,
        AverageFps = 60,
        MinFps = 55,
        AverageFrameTime = 16.6,
        MaxFrameTime = 20,
        ObjectsCount = 10,
        DrawCalls = 5,
        Triangles = 1000,
        Geometries = 3,
        Textures = 2,
        MemoryUsedMb = 128
    };
}