using Boutique.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class UserValidator:AbstractValidator<User> {
        public UserValidator() {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Email is not valid.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Password is required.");
        }
    }
}
