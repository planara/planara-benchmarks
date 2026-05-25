using FluentValidation;
using Planara.Benchmarks.Requests;

namespace Planara.Benchmarks.Validators;

public class GetBenchmarkRunRequestValidator : AbstractValidator<GetBenchmarkRunRequest>
{
    public GetBenchmarkRunRequestValidator()
    {
        RuleFor(x => x.RunId)
            .NotEmpty()
            .WithMessage("ID запуска является обязательным.");
    }
}