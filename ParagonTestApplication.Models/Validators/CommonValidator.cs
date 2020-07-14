using System;
using System.Globalization;
using FluentValidation;

namespace ParagonTestApplication.Models.Validators
{
    public class CommonValidator<T> : AbstractValidator<T>
    {
        protected bool BeAValidDate(string date)
        {
            //return DateTime.TryParseExact(date, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
            return DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
            //2020-07-14T14:49
        }
        
        protected bool BeAValidInt(string value)
        {
            return int.TryParse(value, out _);
        }
    }
}