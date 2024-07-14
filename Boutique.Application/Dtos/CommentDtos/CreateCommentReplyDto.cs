using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.CommentDtos {
    public class CreateCommentReplyDto {
        public string CommentId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
    }
}
