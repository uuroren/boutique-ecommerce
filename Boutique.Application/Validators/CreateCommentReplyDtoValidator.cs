using Boutique.Application.Dtos.CommentDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Validators {
    public class CreateCommentReplyDtoValidator:AbstractValidator<CreateCommentReplyDto> {
        public CreateCommentReplyDtoValidator() {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.CommentId).NotEmpty().WithMessage("Comment ID is required.");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Text is required.");
        }
    }
}
