namespace ParagonTestApplication.Models.Validators
{
    using FluentValidation;
    using ParagonTestApplication.Models.ApiModels.Webinars;

    /// <summary>
    /// Validator for WebinarFilter.
    /// </summary>
    public class WebinarFilterValidator : CommonValidator<WebinarFilter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarFilterValidator"/> class.
        /// </summary>
        public WebinarFilterValidator()
        {
            this.RuleFor(x => x.MinDateTime)
                .Must(this.BeAValidDate).WithMessage("MinDateTime must be in format 2010-01-11T11:41")
                .When(x => x.MinDateTime != null);

            this.RuleFor(x => x.MaxDateTime)
                .Must(this.BeAValidDate).WithMessage("MaxDateTime must be in format 2010-01-11T11:41")
                .When(x => x.MaxDateTime != null);

            this.RuleFor(x => x.MinDuration)
                .Must(this.BeAValidPositiveInt).WithMessage("MinDuration must be a positive valid integer")
                .When(x => x.MinDuration != null);

            this.RuleFor(x => x.MaxDuration)
                .Must(this.BeAValidPositiveInt).WithMessage("MaxDuration must be a positive valid integer")
                .When(x => x.MaxDuration != null);

            this.RuleFor(x => x.SeriesId)
                .Must(this.BeAValidPositiveInt).WithMessage("SeriesId must be a positive valid integer")
                .When(x => x.SeriesId != null);
        }
    }
}