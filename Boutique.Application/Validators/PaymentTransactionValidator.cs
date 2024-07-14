using Boutique.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class PaymentTransactionValidator:AbstractValidator<PaymentTransaction> {
        public PaymentTransactionValidator() {
            RuleFor(x => x.TransactionId).NotEmpty().WithMessage("Transaction ID is required.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.");
        }
    }
}
