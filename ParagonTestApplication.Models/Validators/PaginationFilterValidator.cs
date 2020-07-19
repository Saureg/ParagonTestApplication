namespace ParagonTestApplication.Models.Validators
{
    using FluentValidation;
    using ParagonTestApplication.Models.Common;

    /// <summary>
    /// Validator for PaginationFilter.
    /// </summary>
    public class PaginationFilterValidator : CommonValidator<PaginationFilter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilterValidator"/> class.
        /// </summary>
        public PaginationFilterValidator()
        {
            this.RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be equal or greater than 1")
                .LessThan(int.MaxValue).WithMessage("PageNumber must be less than 12147483647");

            this.RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be equal or greater than 1")
                .LessThan(int.MaxValue).WithMessage("PageSize must be less than 12147483647");
        }
    }
}