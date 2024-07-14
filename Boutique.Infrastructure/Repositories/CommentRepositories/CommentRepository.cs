using Boutique.Domain.Entities;
using Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.CommentRepositories {
    public class CommentRepository:ICommentRepository {
        private readonly IMongoCollection<Comment> _comments;

        public CommentRepository(MongoDbContext context) {
            _comments = context.Comments;
        }

        public async Task CreateCommentAsync(Comment comment) {
            comment.CreatedAt = DateTime.UtcNow.AddHours(3);
            comment.UpdatedAt = DateTime.UtcNow.AddHours(3);
            await _comments.InsertOneAsync(comment);
        }

        public async Task AddReplyToCommentAsync(string commentId,CommentReply reply) {
            var filter = Builders<Comment>.Filter.Eq(c => c.CommentId,commentId);
            var update = Builders<Comment>.Update.Push(c => c.Replies,reply);

            reply.CreatedAt = DateTime.UtcNow.AddHours(3);
            reply.UpdatedAt = DateTime.UtcNow.AddHours(3);

            await _comments.UpdateOneAsync(filter,update);
        }

        public async Task UpdateCommentAsync(Comment comment) {
            var filter = Builders<Comment>.Filter.Eq(c => c.CommentId,comment.CommentId);

            comment.UpdatedAt = DateTime.UtcNow.AddHours(3);

            await _comments.ReplaceOneAsync(filter,comment);
        }

        public async Task<Comment> GetCommentByIdAsync(string commentId) {
            return await _comments.Find(c => c.CommentId == commentId).FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetCommentsByProductIdAsync(string productId) {
            return await _comments.Find(c => c.ProductId == productId).ToListAsync();
        }
    }

}
