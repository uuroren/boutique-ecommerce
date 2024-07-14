using AutoMapper;
using Boutique.Application.Dtos.CartDtos;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.Repositories.CartRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.CartServices {
    public class CartService:ICartService {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository,IMapper mapper) {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> CreateCartAsync(CreateCartDto createCartDto) {
            var existingCart = await _cartRepository.GetCartByUserIdAsync(createCartDto.UserId);
            if(existingCart != null) {
                // Kullanıcının zaten bir sepeti varsa yeni bir sepet oluşturmaz.
                return _mapper.Map<CartDto>(existingCart);
            }

            var newCart = _mapper.Map<Cart>(createCartDto);
            await _cartRepository.CreateCartAsync(newCart);

            return _mapper.Map<CartDto>(newCart);
        }

        public async Task AddOrUpdateCartItemAsync(string userId,CartItemDto newItemDto) {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if(cart == null) {
                throw new Exception("Cart not found");
            }

            var newItem = _mapper.Map<CartItem>(newItemDto);

            var existingItem = cart.Items.Find(i =>
                i.ProductId == newItem.ProductId &&
                i.Variant.Size == newItem.Variant.Size &&
                i.Variant.Color == newItem.Variant.Color
            );

            if(existingItem != null) {
                existingItem.Quantity += newItem.Quantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
            } else {
                newItem.ProductId = newItemDto.ProductId;
                newItem.CreatedAt = DateTime.UtcNow;
                newItem.UpdatedAt = DateTime.UtcNow;
                cart.Items.Add(newItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;

            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task RemoveCartItemAsync(string userId,string cartItemId) {
            await _cartRepository.RemoveCartItemAsync(userId,cartItemId);
        }

        public async Task<CartDto> GetCartByUserIdAsync(string userId) {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return _mapper.Map<CartDto>(cart);
        }
    }
}
