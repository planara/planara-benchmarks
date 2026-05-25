using FluentValidation;
using Planara.Benchmarks.Requests;

namespace Planara.Benchmarks.Validators;

public class DeleteBenchmarkRunRequestValidator : AbstractValidator<DeleteBenchmarkRunRequest>
{
    public DeleteBenchmarkRunRequestValidator()
    {
        RuleFor(x => x.RunId)
            .NotEmpty()
            .WithMessage("ID запуска является обязательным.");
    }
}