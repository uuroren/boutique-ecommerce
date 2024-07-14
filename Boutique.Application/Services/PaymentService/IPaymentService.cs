using Boutique.Application.Dtos.OrderDtos;
using Boutique.Application.Dtos.PaymentDtos;
using Boutique.Domain.Entities;
using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.PaymentService {
    public interface IPaymentService {
        Task<PaymentResponseDTO> Create3DPaymentAsync(Cart cart,Dtos.PaymentDtos.PaymentCard paymentCard);
        Task<OrderResultDto> HandlePaymentCallbackAsync(string paymentToken);
    }
}
