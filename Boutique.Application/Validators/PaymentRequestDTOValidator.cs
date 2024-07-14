using Boutique.Application.Dtos.PaymentDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class PaymentRequestDTOValidator:AbstractValidator<PaymentRequestDTO> {
        public PaymentRequestDTOValidator() {
            RuleFor(x => x.CartId).NotEmpty().WithMessage("Cart ID is required.");
            RuleFor(x => x.PaymentCard).NotEmpty().WithMessage("Payment card is required.");
        }
    }
}
