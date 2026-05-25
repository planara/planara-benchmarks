using FluentValidation;
using Planara.Benchmarks.Requests;

namespace Planara.Benchmarks.Validators;

public class SaveBenchmarkRunRequestValidator : AbstractValidator<SaveBenchmarkRunRequest>
{
    public SaveBenchmarkRunRequestValidator()
    {
        RuleFor(x => x.DurationMs)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Длительность запуска не может быть отрицательной.");

        RuleFor(x => x.UserAgent)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.UserAgent))
            .WithMessage("User-Agent должен быть максимум 512 символов.");

        RuleFor(x => x.Tests)
            .NotEmpty()
            .WithMessage("Запуск должен содержать хотя бы один тест.");

        RuleForEach(x => x.Tests)
            .SetValidator(new SaveBenchmarkTestResultRequestValidator());
    }
}