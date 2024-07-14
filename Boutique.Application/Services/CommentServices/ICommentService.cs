using Boutique.Application.Dtos.CommentDtos;
using Boutique.Application.Dtos;
using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.CommentServices {
    public interface ICommentService {
        Task CreateCommentAsync(CreateCommentDto commentDto);
        Task CreateCommentReplyAsync(CreateCommentReplyDto replyDto);
        Task LikeCommentAsync(string commentId);
        Task<List<Comment>> GetCommentsByProductIdAsync(string productId);
    }
}
