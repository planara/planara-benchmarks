using System.Security.Claims;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Planara.Benchmarks.Data;
using Planara.Benchmarks.Data.Domain;
using Planara.Benchmarks.Requests;
using Planara.Benchmarks.Responses;
using Planara.Benchmarks.Validators;
using Planara.Common.Auth.Claims;

namespace Planara.Benchmarks.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutation
{
    /// <summary>
    /// Сохранение результата запуска бенчмарка
    /// </summary>
    [Authorize]
    [GraphQLDescription("Сохранить результат запуска бенчмарка")]
    public async Task<BenchmarkRunResponse> SaveBenchmarkRun(
        [GraphQLDescription("Данные результата запуска")]
        [UseFluentValidation, UseValidator<SaveBenchmarkRunRequestValidator>]
        SaveBenchmarkRunRequest request,
        ClaimsPrincipal claimsPrincipal,
        [Service] DataContext dataContext,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();

        var run = new BenchmarkRun
        {
            UserId = userId,
            CompletedAt = request.CompletedAt,
            DurationMs = request.DurationMs,
            Status = request.Status,
            UserAgent = request.UserAgent,
            DevicePixelRatio = request.DevicePixelRatio,
            Tests = request.Tests
                .Select(x => new BenchmarkTestResult
                {
                    Type = x.Type,
                    Status = x.Status,
                    ErrorMessage = x.ErrorMessage,
                    DurationMs = x.DurationMs,
                    Frames = x.Frames,
                    AverageFps = x.AverageFps,
                    MinFps = x.MinFps,
                    AverageFrameTime = x.AverageFrameTime,
                    MaxFrameTime = x.MaxFrameTime,
                    ObjectsCount = x.ObjectsCount,
                    DrawCalls = x.DrawCalls,
                    Triangles = x.Triangles,
                    Geometries = x.Geometries,
                    Textures = x.Textures,
                    MemoryUsedMb = x.MemoryUsedMb,
                    History = x.History
                })
                .ToArray()
        };

        await dataContext.BenchmarkRuns.AddAsync(run, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new BenchmarkRunResponse(run);
    }

    /// <summary>
    /// Удаление запуска бенчмарка
    /// </summary>
    [Authorize]
    [GraphQLDescription("Удалить запуск бенчмарка")]
    public async Task<DeleteBenchmarkRunResponse> DeleteBenchmarkRun(
        [GraphQLDescription("Данные для удаления запуска")]
        [UseFluentValidation, UseValidator<DeleteBenchmarkRunRequestValidator>]
        DeleteBenchmarkRunRequest request,
        ClaimsPrincipal claimsPrincipal,
        [Service] DataContext dataContext,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();

        var run = await dataContext.BenchmarkRuns
            .SingleOrDefaultAsync(
                x => x.UserId == userId && x.Id == request.RunId,
                cancellationToken);

        if (run is null)
            return new DeleteBenchmarkRunResponse { Success = false };

        dataContext.BenchmarkRuns.Remove(run);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new DeleteBenchmarkRunResponse { Success = true };
    }
}