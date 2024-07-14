using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.CartRepositories {
    public interface ICartRepository {
        Task CreateCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<Cart> GetCartByIdAsync(string cartId);

        Task RemoveCartItemAsync(string userId,string cartItemId);
    }
}
