using Microsoft.EntityFrameworkCore;
using Planara.Benchmarks.Data;

namespace Planara.Benchmarks.Tests;

public static class DbTestUtils
{
    public static async Task ResetBenchmarksDbAsync(
        DataContext db,
        CancellationToken cancellationToken = default)
    {
        await db.Database.ExecuteSqlRawAsync(
            """
            TRUNCATE TABLE "BenchmarkTestResults", "BenchmarkRuns" RESTART IDENTITY CASCADE;
            """,
            cancellationToken);
    }
}