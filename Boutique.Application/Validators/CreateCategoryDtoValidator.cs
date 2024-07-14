using Boutique.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {

    public class CreateCategoryDtoValidator:AbstractValidator<CreateCategoryDto> {
        public CreateCategoryDtoValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required.");
        }
    }
}
