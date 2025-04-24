using FluentValidation;
using Ambev.DeveloperEvaluation.Application.DTOs;

public class SaleItemCancelDTOValidator : AbstractValidator<SaleItemCancelDTO>
{
    public SaleItemCancelDTOValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do item é obrigatório.");
    }
}
