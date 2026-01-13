using Desafio_WebAPI.Models.Entities;
using Desafio_WebAPI.Models.Request;
using FluentValidation;

namespace Desafio_WebAPI.Models.Validators;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(3).WithMessage("Descrição deve ter no mínimo 3 caracteres.")
            .MaximumLength(100).WithMessage("Descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CategoryType)
            .Must(value => Enum.IsDefined(value))
            .WithMessage("Categoria inválida.");
    }

}
