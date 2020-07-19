namespace ParagonTestApplication.Models.Validators
{
    using System;
    using System.Globalization;
    using FluentValidation;

    /// <summary>
    /// Common validator.
    /// </summary>
    /// <typeparam name="T">Type for validation.</typeparam>
    public class CommonValidator<T> : AbstractValidator<T>
    {
        /// <summary>
        /// Check for valid date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Is valid.</returns>
        protected bool BeAValidDate(string date)
        {
            return DateTime.TryParseExact(
                date,
                "yyyy-MM-ddTHH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }

        /// <summary>
        /// Check for positive integer.
        /// </summary>
        /// <param name="value">value.</param>
        /// <returns>Is valid.</returns>
        protected bool BeAValidPositiveInt(string value)
        {
            if (int.TryParse(value, out var number))
            {
                return number > 0;
            }

            return false;
        }
    }
}