using Boutique.Application.Dtos.CategoryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class UpdateCategoryDtoValidator:AbstractValidator<UpdateCategoryDto> {
        public UpdateCategoryDtoValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required.");
        }
    }
}
