﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentValidation;
using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.Models.Validators
{
    public class CreateOrUpdateWebinarRequestValidator : CommonValidator<CreateOrUpdateWebinarRequest>
    {
        private readonly IEnumerable<Webinar> _webinars;
        private readonly int? _id;
        
        public CreateOrUpdateWebinarRequestValidator(IEnumerable<Webinar> webinars, int? id = null)
        {
            _webinars = webinars;
            _id = id;

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name has to be between 1 and 50 characters long")
                .Must(IsNameUnique).WithMessage("Name must be unique");
            
            RuleFor(x => x.Series)
                .NotNull().WithMessage("Series is required");

            RuleFor(x => x.Series.Name)
                .NotNull().WithMessage("SeriesName is required")
                .Length(1, 50).WithMessage("SeriesName has to be between 1 and 50 characters long")
                .When(x => x.Series != null);

            RuleFor(x => x.StartDateTime)
                .NotEmpty().WithMessage("StartDateTime is required")
                .Must(BeAValidDate).WithMessage("StartDateTime must be in format 2010-01-11T11:41");

            RuleFor(x => x.Duration)
                .NotNull().WithMessage("Duration is required")
                .GreaterThan(1).WithMessage("Duration must be equal or greater than 1 minute")
                .LessThan(24 * 60).WithMessage("Duration must be less than 24 hours");
        }

        private bool IsNameUnique(string newValue)
        {
            var webinar = _webinars.SingleOrDefault(x => string.Equals(x.Name, newValue, StringComparison.CurrentCultureIgnoreCase));
            if (webinar == null)
            {
                return true;
            }
            return webinar.Id == _id;
        }
    }
}