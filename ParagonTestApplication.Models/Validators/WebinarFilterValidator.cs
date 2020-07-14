using FluentValidation;
using ParagonTestApplication.Models.ApiModels.Webinars;

namespace ParagonTestApplication.Models.Validators
{
    public class WebinarFilterValidator : CommonValidator<WebinarFilter>
    {
        public WebinarFilterValidator()
        {
            RuleFor(x => x.MinDateTime)
                .Must(BeAValidDate).WithMessage("MinDateTime must be in format 2010-01-11T11:41")
                .When(x => x.MinDateTime != null);

            RuleFor(x => x.MaxDateTime)
                .Must(BeAValidDate).WithMessage("MaxDateTime must be in format 2010-01-11T11:41")
                .When(x => x.MaxDateTime != null);

            RuleFor(x => x.MinDuration)
                .Must(BeAValidInt).WithMessage("MinDuration must be a valid number")
                .When(x => x.MinDuration != null);

            RuleFor(x => x.MaxDuration)
                .Must(BeAValidInt).WithMessage("MaxDuration must be a valid number")
                .When(x => x.MaxDuration != null);

            RuleFor(x => x.SeriesId)
                .Must(BeAValidInt).WithMessage("SeriesId must be a valid number")
                .When(x => x.SeriesId != null);
        }
    }
}