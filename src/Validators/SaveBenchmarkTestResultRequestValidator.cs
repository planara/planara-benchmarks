using FluentValidation;
using Planara.Benchmarks.Requests;

namespace Planara.Benchmarks.Validators;

public class SaveBenchmarkTestResultRequestValidator : AbstractValidator<SaveBenchmarkTestResultRequest>
{
    public SaveBenchmarkTestResultRequestValidator()
    {
        RuleFor(x => x.DurationMs)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Длительность теста не может быть отрицательной.");

        RuleFor(x => x.Frames)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество кадров не может быть отрицательным.");

        RuleFor(x => x.AverageFps)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Средний FPS не может быть отрицательным.");

        RuleFor(x => x.MinFps)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Минимальный FPS не может быть отрицательным.");

        RuleFor(x => x.AverageFrameTime)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Среднее время кадра не может быть отрицательным.");

        RuleFor(x => x.MaxFrameTime)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Максимальное время кадра не может быть отрицательным.");

        RuleFor(x => x.ObjectsCount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество объектов не может быть отрицательным.");

        RuleFor(x => x.DrawCalls)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество draw calls не может быть отрицательным.");

        RuleFor(x => x.Triangles)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество треугольников не может быть отрицательным.");

        RuleFor(x => x.Geometries)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество геометрий не может быть отрицательным.");

        RuleFor(x => x.Textures)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество текстур не может быть отрицательным.");

        RuleFor(x => x.MemoryUsedMb)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MemoryUsedMb.HasValue)
            .WithMessage("Использование памяти не может быть отрицательным.");
    }
}