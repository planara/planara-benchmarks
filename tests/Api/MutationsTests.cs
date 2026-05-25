using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Tests.Api;

public class MutationsTests : BaseApiTest
{
    public MutationsTests(ApiTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SaveBenchmarkRun_ValidRequest_SavesRunWithTests()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        const string mutation = """
            mutation SaveBenchmarkRun($request: SaveBenchmarkRunRequestInput!) {
              saveBenchmarkRun(request: $request) {
                id
                durationMs
                status
                userAgent
                devicePixelRatio
                tests {
                  id
                  type
                  status
                  durationMs
                  frames
                  averageFps
                  minFps
                  averageFrameTime
                  maxFrameTime
                  objectsCount
                  drawCalls
                  triangles
                  geometries
                  textures
                  memoryUsedMb
                  history {
                    timeMs
                    averageFps
                    minFps
                    averageFrameTime
                    maxFrameTime
                    memoryUsedMb
                    drawCalls
                    triangles
                    objectsCount
                  }
                }
              }
            }
            """;

        var variables = new
        {
            request = new
            {
                completedAt = DateTime.UtcNow,
                durationMs = 1000.0,
                status = "COMPLETED",
                userAgent = "Tests",
                devicePixelRatio = 2.0,
                tests = new[]
                {
                    new
                    {
                        type = "LIGHT",
                        status = "SUCCESS",
                        errorMessage = (string?)null,
                        durationMs = 1000.0,
                        frames = 60,
                        averageFps = 60.0,
                        minFps = 55.0,
                        averageFrameTime = 16.6,
                        maxFrameTime = 20.0,
                        objectsCount = 10,
                        drawCalls = 5,
                        triangles = 1000,
                        geometries = 3,
                        textures = 2,
                        memoryUsedMb = 128.5,
                        history = new
                        {
                            timeMs = new[] { 0.0, 500.0, 1000.0 },
                            averageFps = new[] { 60.0, 59.0, 58.0 },
                            minFps = new[] { 55.0, 54.0, 53.0 },
                            averageFrameTime = new[] { 16.6, 16.8, 17.0 },
                            maxFrameTime = new[] { 20.0, 21.0, 22.0 },
                            memoryUsedMb = new double?[] { 120.0, 124.0, 128.5 },
                            drawCalls = new[] { 5, 5, 6 },
                            triangles = new[] { 1000, 1000, 1100 },
                            objectsCount = new[] { 10, 10, 11 }
                        }
                    }
                }
            }
        };

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var run = json.GetData().GetProperty("saveBenchmarkRun");

        run.GetProperty("durationMs").GetDouble().Should().Be(1000.0);
        run.GetProperty("status").GetString().Should().Be("COMPLETED");
        run.GetProperty("userAgent").GetString().Should().Be("Tests");
        run.GetProperty("devicePixelRatio").GetDouble().Should().Be(2.0);

        var tests = run.GetProperty("tests").EnumerateArray().ToArray();

        tests.Should().HaveCount(1);
        tests[0].GetProperty("type").GetString().Should().Be("LIGHT");
        tests[0].GetProperty("status").GetString().Should().Be("SUCCESS");
        tests[0].GetProperty("averageFps").GetDouble().Should().Be(60.0);

        var history = tests[0].GetProperty("history");

        history.GetProperty("timeMs").EnumerateArray().Should().HaveCount(3);
        history.GetProperty("memoryUsedMb").EnumerateArray().Should().HaveCount(3);

        var savedRuns = await Context.BenchmarkRuns.CountAsync();
        var savedTests = await Context.BenchmarkTestResults.CountAsync();

        savedRuns.Should().Be(1);
        savedTests.Should().Be(1);
    }

    [Fact]
    public async Task DeleteBenchmarkRun_ExistingRun_RemovesRunWithTests()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        var run = new BenchmarkRun
        {
            UserId = UserId,
            CompletedAt = DateTime.UtcNow,
            DurationMs = 1000,
            Status = BenchmarkRunStatus.Completed,
            UserAgent = "Tests",
            DevicePixelRatio = 2,
            Tests =
            [
                new BenchmarkTestResult
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
                    MemoryUsedMb = 128,
                    History = new BenchmarkMetricsHistory
                    {
                        TimeMs = [0, 1000],
                        AverageFps = [60, 58]
                    }
                }
            ]
        };

        Context.BenchmarkRuns.Add(run);
        await Context.SaveChangesAsync();

        const string mutation = """
            mutation DeleteBenchmarkRun($request: DeleteBenchmarkRunRequestInput!) {
              deleteBenchmarkRun(request: $request) {
                success
              }
            }
            """;

        var variables = new
        {
            request = new
            {
                runId = run.Id
            }
        };

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var result = json.GetData().GetProperty("deleteBenchmarkRun");

        result.GetProperty("success").GetBoolean().Should().BeTrue();

        var runExists = await Context.BenchmarkRuns.AnyAsync(x => x.Id == run.Id);
        var testsCount = await Context.BenchmarkTestResults.CountAsync();

        runExists.Should().BeFalse();
        testsCount.Should().Be(0);
    }

    [Fact]
    public async Task DeleteBenchmarkRun_ForeignRun_ReturnsFalse()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        var foreignRun = new BenchmarkRun
        {
            UserId = Guid.NewGuid(),
            CompletedAt = DateTime.UtcNow,
            DurationMs = 1000,
            Status = BenchmarkRunStatus.Completed
        };

        Context.BenchmarkRuns.Add(foreignRun);
        await Context.SaveChangesAsync();

        const string mutation = """
            mutation DeleteBenchmarkRun($request: DeleteBenchmarkRunRequestInput!) {
              deleteBenchmarkRun(request: $request) {
                success
              }
            }
            """;

        var variables = new
        {
            request = new
            {
                runId = foreignRun.Id
            }
        };

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var result = json.GetData().GetProperty("deleteBenchmarkRun");

        result.GetProperty("success").GetBoolean().Should().BeFalse();

        var runExists = await Context.BenchmarkRuns.AnyAsync(x => x.Id == foreignRun.Id);
        runExists.Should().BeTrue();
    }
}