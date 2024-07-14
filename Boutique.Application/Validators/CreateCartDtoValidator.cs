using Boutique.Application.Dtos.CartDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class CreateCartDtoValidator:AbstractValidator<CreateCartDto> {
        public CreateCartDtoValidator() {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
         
        }
    }
}
