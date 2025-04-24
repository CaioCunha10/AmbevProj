using FluentValidation;
using Ambev.DeveloperEvaluation.Application.DTOs;

public class SalePutDTOValidator : AbstractValidator<SalePutDTO>
{
    public SalePutDTOValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty().WithMessage("O número da venda é obrigatório.");

        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("O valor total não pode ser negativo.");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("A filial é obrigatória.");

        RuleForEach(x => x.Items).SetValidator(new SaleItemPutDTOValidator());
    }
}

public class SaleItemPutDTOValidator : AbstractValidator<SaleItemPutDTO>
{
    public SaleItemPutDTOValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Product).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);
    }
}
