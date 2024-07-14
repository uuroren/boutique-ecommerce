using Boutique.Application.Dtos.OrderDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class OrderDTOValidator:AbstractValidator<OrderDTO> {
        public OrderDTOValidator() {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("Order must contain at least one item.");
            RuleForEach(x => x.Items).SetValidator(new OrderItemDTOValidator());
            RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        }
    }
}
