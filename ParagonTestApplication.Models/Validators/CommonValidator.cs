using System;
using System.Globalization;
using FluentValidation;

namespace ParagonTestApplication.Models.Validators
{
    public class CommonValidator<T> : AbstractValidator<T>
    {
        protected bool BeAValidDate(string date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out _);
        }

        protected bool BeAValidInt(string value)
        {
            return int.TryParse(value, out _);
        }
    }
}