using FluentValidation;

namespace ABPTestApp.Models.DTOs.Validators
{
    public class ExperimentRequestValidator : AbstractValidator<ExperimentRequest>
    {
        public ExperimentRequestValidator()
        {
            RuleFor(p => p.DeviceToken).NotEmpty().MaximumLength(100);
        }
    }
}
