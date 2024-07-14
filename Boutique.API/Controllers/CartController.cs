using Boutique.API.Models;
using Boutique.Application.Dtos;
using Boutique.Application.Dtos.CartDtos;
using Boutique.Application.Services.CartServices;
using Boutique.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CartController:ControllerBase {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService) {
        _cartService = cartService;
    }

    [HttpPost("create-cart")]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartDto createCartDto) {
        var cart = await _cartService.CreateCartAsync(createCartDto);
        return Ok(new BaseResponse() { Message = "Card created.",Result = cart,Success = true,});
    }

    [HttpPost("add-item")]
    public async Task<IActionResult> AddOrUpdateCartItem([FromQuery] string userId,[FromBody] CartItemDto cartItemDto) {
        await _cartService.AddOrUpdateCartItemAsync(userId,cartItemDto);
        return Ok(new BaseResponse() { Message = "The product has been created.",Success = true,Result = cartItemDto,});
    }

    [HttpDelete("remove-item")]
    public async Task<IActionResult> RemoveCartItem([FromQuery] string userId,[FromQuery] string cartItemId) {
        await _cartService.RemoveCartItemAsync(userId,cartItemId);

        return Ok(new BaseResponse() { Message = "The product has been deleted",Success = true,});
    }

    [HttpGet("get-cart")]
    public async Task<IActionResult> GetCartByUserId([FromQuery] string userId) {
        var cart = await _cartService.GetCartByUserIdAsync(userId);

        return Ok(new BaseResponse() { Result = cart,Success = true,});
    }
}
