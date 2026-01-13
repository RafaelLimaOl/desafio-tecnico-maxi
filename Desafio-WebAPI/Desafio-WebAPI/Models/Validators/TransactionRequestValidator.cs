using Desafio_WebAPI.Models.Request;
using FluentValidation;

namespace Desafio_WebAPI.Models.Validators;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatório.")
            .MinimumLength(3).WithMessage("Descrição deve ter no mínimo 3 caracteres.")
            .MaximumLength(200).WithMessage("Descrição deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Valor deve ser maior que 0.");

        RuleFor(x => x.PeopleId)
            .NotEqual(Guid.Empty)
            .WithMessage("Pessoa Id é obrigatório.");

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage("Categoria Id é obrigatório.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status inválido.");

        RuleFor(x => x.TransactionType)
            .IsInEnum()
            .WithMessage("Tipo de transacação inválida.");
    }
}
