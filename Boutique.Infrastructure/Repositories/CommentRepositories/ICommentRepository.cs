using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.CommentRepositories {
    public interface ICommentRepository {
        Task CreateCommentAsync(Comment comment);
        Task AddReplyToCommentAsync(string commentId,CommentReply reply);
        Task UpdateCommentAsync(Comment comment);
        Task<Comment> GetCommentByIdAsync(string commentId);
        Task<List<Comment>> GetCommentsByProductIdAsync(string productId);
    }
}
