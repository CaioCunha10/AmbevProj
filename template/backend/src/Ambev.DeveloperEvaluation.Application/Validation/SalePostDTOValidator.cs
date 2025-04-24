using FluentValidation;
using Ambev.DeveloperEvaluation.Application.DTOs;

public class SalePostDTOValidator : AbstractValidator<SalePostDTO>
{
    public SalePostDTOValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data da venda é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data não pode ser futura.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("A venda deve conter pelo menos um item.");

        RuleForEach(x => x.Items)
            .NotEmpty().WithMessage("Os itens da venda são obrigatórios.");
    }
}
