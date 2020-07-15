using FluentValidation;
using ParagonTestApplication.Models.Common;

namespace ParagonTestApplication.Models.Validators
{
    public class PaginationFilterValidator : CommonValidator<PaginationFilter>
    {
        public PaginationFilterValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be equal or greater than 1")
                .LessThan(int.MaxValue).WithMessage("PageNumber must be less than 12147483647");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be equal or greater than 1")
                .LessThan(int.MaxValue).WithMessage("PageSize must be less than 12147483647");
        }
    }
}