using Boutique.Application.Dtos.OrderDtos;
using Boutique.Application.Dtos.PaymentDtos;
using Boutique.Application.Services.PaymentService;
using Boutique.Application.Services.RabbitMQServices;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.Repositories.CartRepositories;
using Boutique.Infrastructure.Repositories.OrderRepositories;
using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.OrderServices {
    public class OrderService:IOrderService {
        private readonly IPaymentService _paymentService;
        private readonly ICartRepository _cartRepository;

        public OrderService(IPaymentService paymentService,ICartRepository cartRepository) {
            _paymentService = paymentService;
            _cartRepository = cartRepository;
        }

        public async Task<PaymentResponseDTO> CreateOrderAsync(string cartId,Dtos.PaymentDtos.PaymentCard paymentCard) {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if(cart == null) throw new Exception("Cart not found");

            return await _paymentService.Create3DPaymentAsync(cart,paymentCard);
        }

        public async Task<OrderResultDto> HandlePaymentCallbackAsync(string paymentToken) {
            return await _paymentService.HandlePaymentCallbackAsync(paymentToken);
        }
    }

}
