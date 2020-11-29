using Demo.UI.ViewModels;
using FluentValidation;

namespace Demo.UI.Validators
{
    public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {
            RuleFor(c => c.ProductName).NotEmpty().WithMessage("Please enter Product Name.");
            RuleFor(c => c.QuantityPerUnit).NotEmpty().WithMessage("Please enter Quantity per Unit.");
            RuleFor(c => c.UnitPrice).NotEmpty().WithMessage("Please enter Unit Price.");
        }
    }
}
