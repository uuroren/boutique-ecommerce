using Boutique.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class CreateCommentDtoValidator:AbstractValidator<CreateCommentDto> {
        public CreateCommentDtoValidator() {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Text is required.");
        }
    }
}
