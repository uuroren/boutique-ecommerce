using Boutique.Application.Dtos.CartDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class CartDtoValidator:AbstractValidator<CartDto> {
        public CartDtoValidator() {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("Cart must contain at least one item.");
            RuleForEach(x => x.Items).SetValidator(new CartItemDtoValidator());
        }
    }
}
