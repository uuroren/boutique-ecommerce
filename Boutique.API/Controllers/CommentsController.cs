using Boutique.Application.Dtos.CommentDtos;
using Boutique.Application.Dtos;
using Boutique.Application.Services.CommentServices;
using Boutique.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Boutique.API.Models;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController:ControllerBase {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService) {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateComment([FromForm] CreateCommentDto commentDto) {
            await _commentService.CreateCommentAsync(commentDto);
            return Ok(new BaseResponse { Message = "Comment added successfully",Success = true });
        }

        [Authorize]
        [HttpPost("reply")]
        public async Task<ActionResult> CreateCommentReply([FromBody] CreateCommentReplyDto replyDto) {
            await _commentService.CreateCommentReplyAsync(replyDto);
            return Ok(new BaseResponse { Message = "Reply added successfully",Success = true });
        }

        [Authorize]
        [HttpPost("{commentId}/like")]
        public async Task<ActionResult> LikeComment(string commentId) {
            await _commentService.LikeCommentAsync(commentId);
            return Ok(new BaseResponse { Message = "Comment liked successfully",Success = true });
        }


        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByProductId(string productId) {
            var comments = await _commentService.GetCommentsByProductIdAsync(productId);
            return Ok(new BaseResponse { Result = comments,Success = true });
        }
    }
}
