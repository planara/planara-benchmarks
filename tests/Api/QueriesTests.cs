using FluentAssertions;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Data.Enums;

namespace Planara.Benchmarks.Tests.Api;

public class QueriesTests : BaseApiTest
{
    public QueriesTests(ApiTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetMyBenchmarkRuns_ReturnsOnlyCurrentUserRuns()
    {
        await DbTestUtils.ResetBenchmarksDbAsync(Context);

        Context.BenchmarkRuns.AddRange(
            new BenchmarkRun
            {
                UserId = UserId,
                CompletedAt = DateTime.UtcNow,
                DurationMs = 1000,
                Status = BenchmarkRunStatus.Completed,
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
                        Textures = 2
                    }
                ]
            },
            new BenchmarkRun
            {
                UserId = Guid.NewGuid(),
                CompletedAt = DateTime.UtcNow,
                DurationMs = 2000,
                Status = BenchmarkRunStatus.Completed
            }
        );

        await Context.SaveChangesAsync();

        const string query = """
            query MyBenchmarkRuns {
              myBenchmarkRuns(first: 20) {
                totalCount
                nodes {
                  id
                  durationMs
                  status
                  testsCount
                }
              }
            }
            """;

        using var json = await Client.PostAsync(query);

        json.GetErrors().Should().BeNull();

        var runs = json.GetData().GetProperty("myBenchmarkRuns");

        runs.GetProperty("totalCount").GetInt32().Should().Be(1);

        var nodes = runs.GetProperty("nodes").EnumerateArray().ToArray();

        nodes.Should().HaveCount(1);
        nodes[0].GetProperty("durationMs").GetDouble().Should().Be(1000);
        nodes[0].GetProperty("status").GetString().Should().Be("COMPLETED");
        nodes[0].GetProperty("testsCount").GetInt32().Should().Be(1);
    }

    [Fact]
    public async Task GetBenchmarkRun_ExistingRun_ReturnsRunWithTests()
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
                    Type = BenchmarkTestType.Heavy,
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
                        AverageFps = [60, 58],
                        MinFps = [55, 53],
                        AverageFrameTime = [16.6, 17],
                        MaxFrameTime = [20, 22],
                        MemoryUsedMb = [120, 128],
                        DrawCalls = [5, 6],
                        Triangles = [1000, 1100],
                        ObjectsCount = [10, 11]
                    }
                }
            ]
        };

        Context.BenchmarkRuns.Add(run);
        await Context.SaveChangesAsync();

        const string query = """
            query BenchmarkRun($request: GetBenchmarkRunRequestInput!) {
              benchmarkRun(request: $request) {
                id
                durationMs
                status
                userAgent
                devicePixelRatio
                tests {
                  type
                  status
                  averageFps
                  history {
                    timeMs
                    averageFps
                  }
                }
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

        using var json = await Client.PostAsync(query, variables);

        json.GetErrors().Should().BeNull();

        var result = json.GetData().GetProperty("benchmarkRun");

        result.GetProperty("id").GetGuid().Should().Be(run.Id);
        result.GetProperty("durationMs").GetDouble().Should().Be(1000);
        result.GetProperty("status").GetString().Should().Be("COMPLETED");

        var tests = result.GetProperty("tests").EnumerateArray().ToArray();

        tests.Should().HaveCount(1);
        tests[0].GetProperty("type").GetString().Should().Be("HEAVY");
        tests[0].GetProperty("status").GetString().Should().Be("SUCCESS");
        tests[0].GetProperty("averageFps").GetDouble().Should().Be(60);

        tests[0]
            .GetProperty("history")
            .GetProperty("timeMs")
            .EnumerateArray()
            .Should()
            .HaveCount(2);
    }

    [Fact]
    public async Task GetBenchmarkRun_ForeignRun_ReturnsError()
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

        const string query = """
            query BenchmarkRun($request: GetBenchmarkRunRequestInput!) {
              benchmarkRun(request: $request) {
                id
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

        using var json = await Client.PostAsync(query, variables);

        json.GetErrors().Should().NotBeNull();
    }
}