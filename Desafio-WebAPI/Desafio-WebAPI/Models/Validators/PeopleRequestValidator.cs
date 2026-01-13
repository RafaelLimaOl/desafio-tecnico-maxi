using Desafio_WebAPI.Models.Request;
using FluentValidation;

namespace Desafio_WebAPI.Models.Validators;

public class PeopleRequestValidator : AbstractValidator<PeopleRequest>
{
    public PeopleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Idade deve ser maior que 0.")
            .LessThanOrEqualTo(120).WithMessage("Idade deve ser menor ou igual a 120.");
    }
}