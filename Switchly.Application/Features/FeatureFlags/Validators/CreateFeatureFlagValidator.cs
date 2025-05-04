using FluentValidation;

namespace Switchly.Application.FeatureFlags.Commands.CreateFeatureFlag;

public class CreateFeatureFlagValidator : AbstractValidator<CreateFeatureFlagCommand>
{
    public CreateFeatureFlagValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("OrganizationId boş olamaz.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key boş olamaz.")
            .MaximumLength(100).WithMessage("Key en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
    }
}