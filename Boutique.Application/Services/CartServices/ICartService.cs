using Boutique.Application.Dtos.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.CartServices {
    public interface ICartService {
        Task<CartDto> CreateCartAsync(CreateCartDto createCartDto);
        Task AddOrUpdateCartItemAsync(string userId,CartItemDto newItemDto);
        Task RemoveCartItemAsync(string userId,string cartItemId);
        Task<CartDto> GetCartByUserIdAsync(string userId);
    }
}
