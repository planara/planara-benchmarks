using Microsoft.EntityFrameworkCore;
using Planara.Benchmarks.Data.Domain;

namespace Planara.Benchmarks.Data;

public class DataContext(DbContextOptions options): DbContext(options)
{
    public DbSet<BenchmarkRun> BenchmarkRuns { get; set; } = null!;
    public DbSet<BenchmarkTestResult> BenchmarkTestResults { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<BenchmarkRun>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.UserAgent)
                .HasMaxLength(512);

            builder.HasMany(x => x.Tests)
                .WithOne(x => x.Run)
                .HasForeignKey(x => x.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.CreatedAt
            });
        });

        modelBuilder.Entity<BenchmarkTestResult>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(2000);

            builder.OwnsOne(x => x.History, history =>
            {
                history.ToJson();
            });

            builder.HasIndex(x => x.RunId);
        });
    }
}