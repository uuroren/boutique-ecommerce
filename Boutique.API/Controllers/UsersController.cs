using Boutique.API.Models;
using Boutique.Application.Dtos;
using Boutique.Application.Interfaces;
using Boutique.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController:ControllerBase {
        private readonly UserManager<User> _userManager;
        public UsersController(UserManager<User> userManager) {
            _userManager = userManager;
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("block-user/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute] Guid id) {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Blocked = true;

            await _userManager.UpdateAsync(user);

            return Ok(new BaseResponse { Message = "user is blocked",Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-users")]
        public IActionResult GetAllUsersAsync() {
            var users = _userManager.Users.ToList();
            return Ok(new BaseResponse { Result = users,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("get-user/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id) {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return Ok(new BaseResponse { Result = user,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("unblock-user/{id}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] Guid id) {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Blocked = false;

            await _userManager.UpdateAsync(user);

            return Ok(new BaseResponse { Message = "user is unblocked",Success = true });
        }

    }
}
