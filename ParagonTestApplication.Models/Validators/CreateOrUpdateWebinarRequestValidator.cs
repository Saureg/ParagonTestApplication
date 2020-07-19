namespace ParagonTestApplication.Models.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Validator for CreateOrUpdateWebinarRequest model.
    /// </summary>
    public class CreateOrUpdateWebinarRequestValidator : CommonValidator<CreateOrUpdateWebinarRequest>
    {
        private readonly IEnumerable<Webinar> webinars;
        private readonly int? id;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateWebinarRequestValidator"/> class.
        /// </summary>
        /// <param name="webinars">Webinars.</param>
        /// <param name="id">Id.</param>
        public CreateOrUpdateWebinarRequestValidator(IEnumerable<Webinar> webinars, int? id = null)
        {
            this.webinars = webinars;
            this.id = id;

            this.RuleFor(x => x.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name has to be between 1 and 50 characters long")
                .Must(this.IsNameUnique).WithMessage("Name must be unique");

            this.RuleFor(x => x.Series)
                .NotNull().WithMessage("Series is required");

            this.RuleFor(x => x.Series.Name)
                .NotNull().WithMessage("SeriesName is required")
                .Length(1, 50).WithMessage("SeriesName has to be between 1 and 50 characters long")
                .When(x => x.Series != null);

            this.RuleFor(x => x.StartDateTime)
                .NotEmpty().WithMessage("StartDateTime is required");

            this.RuleFor(x => x.StartDateTime)
                .Must(this.BeAValidDate).WithMessage("StartDateTime must be in format 2010-01-11T11:41")
                .When(x => x.StartDateTime != null);

            this.RuleFor(x => x.Duration)
                .GreaterThanOrEqualTo(1).WithMessage("Duration must be equal or greater than 1 minute")
                .LessThanOrEqualTo(24 * 60).WithMessage("Duration must be less than 24 hours");
        }

        private bool IsNameUnique(string newValue)
        {
            var webinar = this.webinars.SingleOrDefault(x =>
                string.Equals(x.Name, newValue, StringComparison.CurrentCultureIgnoreCase));
            if (webinar == null)
            {
                return true;
            }

            return webinar.Id == this.id;
        }
    }
}