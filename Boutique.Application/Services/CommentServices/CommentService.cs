using AutoMapper;
using Boutique.Application.Dtos.CommentDtos;
using Boutique.Application.Dtos;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boutique.Infrastructure.Repositories.CommentRepositories;

namespace Boutique.Application.Services.CommentServices {
    public class CommentService:ICommentService {
        private readonly ICommentRepository _commentRepository;
        private readonly AwsS3Service _awsS3Service;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository,AwsS3Service awsS3Service,IMapper mapper) {
            _commentRepository = commentRepository;
            _awsS3Service = awsS3Service;
            _mapper = mapper;
        }

        public async Task CreateCommentAsync(CreateCommentDto commentDto) {
            var comment = _mapper.Map<Comment>(commentDto);
            comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = DateTime.UtcNow;
            comment.Likes = 0;
            comment.Replies = new List<CommentReply>();

            if(commentDto.Image != null) {
                using(var stream = commentDto.Image.OpenReadStream()) {
                    var imageUrl = await _awsS3Service.UploadFileAsync(stream,commentDto.Image.FileName,commentDto.Image.ContentType);
                    comment.ImageUrl = imageUrl;
                }
            }

            await _commentRepository.CreateCommentAsync(comment);
        }

        public async Task CreateCommentReplyAsync(CreateCommentReplyDto replyDto) {
            var reply = _mapper.Map<CommentReply>(replyDto);

            reply.CreatedAt = DateTime.UtcNow;
            reply.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.AddReplyToCommentAsync(replyDto.CommentId,reply);
        }

        public async Task LikeCommentAsync(string commentId) {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if(comment != null) {
                comment.Likes++;
                await _commentRepository.UpdateCommentAsync(comment);
            }
        }

        public async Task<List<Comment>> GetCommentsByProductIdAsync(string productId) {
            return await _commentRepository.GetCommentsByProductIdAsync(productId);
        }
    }
}
