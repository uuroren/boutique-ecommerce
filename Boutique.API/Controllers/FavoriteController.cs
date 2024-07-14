using AutoMapper;
using Boutique.API.Models;
using Boutique.Application.Dtos.FavoriteDtos;
using Boutique.Application.Services.CartServices;
using Boutique.Application.Services.FavoriteServices;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController:ControllerBase {
        private readonly IFavoriteService _favoriteService;
        private IMapper _mapper;
        public FavoriteController(IFavoriteService favoriteService,IMapper mapper) {
            _favoriteService = favoriteService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("add-favorite")]
        public async Task<IActionResult> AddFavorite(CreateFavoriteDto favorite) {
            var data = _mapper.Map<Favorite>(favorite);
            var success = await _favoriteService.AddFavoriteAsync(data);

            return Ok(new BaseResponse() { Message = success ? "Added to favorites" : "Error",Result = favorite,Success = success });
        }

        [Authorize]
        [HttpPost("get-favorites")]
        public async Task<IActionResult> GetAllFavorites(string userId) {
            var result = await _favoriteService.GetUserFavoritesAsync(userId);
            return Ok(new BaseResponse() { Result = result,Success = true });
        }

        [Authorize]
        [HttpDelete("remove-favorite")]
        public async Task<IActionResult> RemoveFavorite(string userId,string productId) {
            var success = await _favoriteService.RemoveFavoriteAsync(userId,productId);

            return Ok(new BaseResponse() { Success = success,Message = success ? "Removed from favorites" : "Error" });
        }
    }
}
