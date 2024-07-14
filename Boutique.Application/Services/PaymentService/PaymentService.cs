using Amazon.S3.Model;
using AutoMapper;
using Boutique.Application.Dtos.OrderDtos;
using Boutique.Application.Dtos.PaymentDtos;
using Boutique.Application.Services.RabbitMQServices;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.Repositories.AdressRepositories;
using Boutique.Infrastructure.Repositories.CartRepositories;
using Boutique.Infrastructure.Repositories.OrderRepositories;
using Boutique.Infrastructure.Repositories.PaymentRepositories;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.PaymentService {
    public class PaymentService:IPaymentService {
        private readonly Iyzipay.Options _options;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<User> _userManager;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAddressRepository _addressRepository;
        public PaymentService(ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IOptions<IyzicoSettings> iyzicoSettings,
            UserManager<User> userManager,
            IPaymentRepository paymentRepository,
            IAddressRepository addressRepository
            ) {

            var settings = iyzicoSettings.Value;
            _options = new Iyzipay.Options {
                ApiKey = settings.ApiKey,
                SecretKey = settings.SecretKey,
                BaseUrl = settings.BaseUrl
            };

            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _addressRepository = addressRepository;
        }

        public async Task<PaymentResponseDTO> Create3DPaymentAsync(Cart cart,Dtos.PaymentDtos.PaymentCard paymentCard) {
            var user = _userManager.Users.Where(o => o.Id == Guid.Parse(cart.UserId)).FirstOrDefault();

            var userAddress = await _addressRepository.GetUserDefaultAdressAsync(user.Id.ToString());

            var payment = new Domain.Entities.Payment {
                UserId = cart.UserId,
                Amount = cart.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                IsSuccess = false,
                CartId = cart.Id,
            };

            var request = new CreateCheckoutFormInitializeRequest {
                Locale = Locale.TR.ToString(),
                ConversationId = Guid.NewGuid().ToString(),
                Price = cart.TotalAmount.ToString(),
                PaidPrice = cart.TotalAmount.ToString(),
                Currency = Currency.TRY.ToString(),
                EnabledInstallments = new List<int>() {
                    1,2,3,6,9
                },

                BasketId = cart.Id,

                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = "https://d02d-31-223-41-249.ngrok-free.app/api/Payment/callback",

                Buyer = new Buyer {
                    Id = cart.UserId,
                    Name = user.Name,
                    Surname = user.Surname,
                    GsmNumber = user.PhoneNumber,
                    Email = user.Email,
                    IdentityNumber = user.IdentityNumber,
                    RegistrationAddress = user.IdentityNumber,
                    City = userAddress.City,
                    Country = userAddress.Country,
                    ZipCode = userAddress.PostalCode,
                },
                ShippingAddress = new Iyzipay.Model.Address() {
                    City = userAddress.City,
                    ZipCode = userAddress.PostalCode,
                    ContactName = user.Name,
                    Country = userAddress.Country,
                    Description = "TEST",
                },
                BillingAddress = new Iyzipay.Model.Address() {
                    City = userAddress.City,
                    ZipCode = userAddress.PostalCode,
                    ContactName = user.Name,
                    Country = userAddress.Country,
                    Description = "TEST",
                },
                BasketItems = cart.Items.Select(item => new BasketItem {
                    Id = item.Id,
                    Name = item.ProductName,
                    Category1 = "Product",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = item.TotalPrice.ToString()
                }).ToList()
            };

            var checkoutFormInitialize = CheckoutFormInitialize.Create(request,_options);
            if(checkoutFormInitialize.Status == "success") {
                payment.IsSuccess = true;
            } else {
                payment.IsSuccess = false;
                payment.ErrorMessage = checkoutFormInitialize.ErrorMessage;
            }

            payment.PaymentToken = checkoutFormInitialize.Token;

            await _paymentRepository.AddAsync(payment);
            return new PaymentResponseDTO {
                Status = checkoutFormInitialize.Status,
                PaymentPageUrl = checkoutFormInitialize.PaymentPageUrl,
                Token = checkoutFormInitialize.Token,
                ErrorMessage = checkoutFormInitialize.ErrorMessage ?? "",
            };
        }

        public async Task<OrderResultDto> HandlePaymentCallbackAsync(string paymentToken) {
            var request = new RetrieveCheckoutFormRequest {
                ConversationId = Guid.NewGuid().ToString(),
                Token = paymentToken
            };

            Domain.Entities.Order order = null;

            var checkoutForm = CheckoutForm.Retrieve(request,_options);

            var payment = await _paymentRepository.GetByPaymentTokenAsync(checkoutForm.Token);

            if(checkoutForm.Status == "success") {
                // Ödeme başarılıysa siparişi oluştur
                var cart = await _cartRepository.GetCartByIdAsync(checkoutForm.BasketId);
                order = new Domain.Entities.Order {
                    UserId = cart.UserId,
                    Items = cart.Items.Select(i => new Domain.Entities.OrderItem {
                        Id = i.Id,
                        ProductId = i.Id,
                        Quantity = i.Quantity,
                        Price = i.TotalPrice,
                    }).ToList(),
                    TotalAmount = cart.TotalAmount
                };

                await _orderRepository.AddAsync(order);

                RabbitMQHelper.SendMessage(order,"orderQueue");


            } else {
                payment.IsSuccess = false;
                payment.ErrorMessage = checkoutForm.ErrorMessage;

            }

            payment.IsSuccess = checkoutForm.Status == "success";

            payment.Transactions.Add(new PaymentTransaction {
                PaymentId = payment.Id,
                TransactionId = checkoutForm.PaymentId,
                Status = checkoutForm.Status,
                CreatedAt = DateTime.UtcNow,
                Payment = payment,
            });

            await _paymentRepository.AddAsync(payment);

            return new OrderResultDto() { Message = checkoutForm.ErrorMessage,CheckoutForm = checkoutForm,Success = payment.IsSuccess,Order = order };

        }
    }

}
