using System.Security.Claims;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Planara.Benchmarks.Data;
using Planara.Benchmarks.Requests;
using Planara.Benchmarks.Responses;
using Planara.Benchmarks.Validators;
using Planara.Common.Auth.Claims;
using Planara.Common.Exceptions;

namespace Planara.Benchmarks.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public class Query
{
    /// <summary>
    /// Получение запуска бенчмарка по ID
    /// </summary>
    [Authorize]
    [GraphQLDescription("Получить запуск бенчмарка по ID")]
    public async Task<BenchmarkRunResponse> GetBenchmarkRun(
        [Service] DataContext dataContext,
        ClaimsPrincipal claimsPrincipal,
        [GraphQLDescription("Данные для получения запуска")]
        [UseFluentValidation, UseValidator<GetBenchmarkRunRequestValidator>]
        GetBenchmarkRunRequest request,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();

        var run = await dataContext.BenchmarkRuns
            .AsNoTracking()
            .Include(x => x.Tests)
            .Where(x => x.UserId == userId && x.Id == request.RunId)
            .FirstOrDefaultAsync(cancellationToken);

        if (run is null)
            throw new NotFoundException();

        return new BenchmarkRunResponse(run);
    }

    /// <summary>
    /// Получение списка запусков бенчмарков текущего пользователя
    /// </summary>
    [Authorize]
    [UsePaging(MaxPageSize = 50, DefaultPageSize = 20, IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Получить список запусков бенчмарков пользователя")]
    public IQueryable<BenchmarkRunListItemResponse> GetMyBenchmarkRuns(
        [Service] DataContext dataContext,
        ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();

        return dataContext.BenchmarkRuns
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new BenchmarkRunListItemResponse
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                CompletedAt = x.CompletedAt,
                DurationMs = x.DurationMs,
                Status = x.Status,
                TestsCount = x.Tests.Count,
                UserAgent = x.UserAgent,
                DevicePixelRatio = x.DevicePixelRatio
            });
    }
}