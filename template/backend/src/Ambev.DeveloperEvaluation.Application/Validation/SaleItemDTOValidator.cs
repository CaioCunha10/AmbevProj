using FluentValidation;
using Ambev.DeveloperEvaluation.Application.DTOs;

public class SaleItemPostDTOValidator : AbstractValidator<SaleItemDTO>
{
    public SaleItemPostDTOValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("O produto é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
    }
}
