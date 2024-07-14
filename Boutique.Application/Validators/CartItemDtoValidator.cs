using Boutique.Application.Dtos.CartDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class CartItemDtoValidator:AbstractValidator<CartItemDto> {
        public CartItemDtoValidator() {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit  Price must be greater than zero.");
        }
    }
}
