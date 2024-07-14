using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Interfaces {
    public interface IVerificationService {
        Task StoreVerificationCodeAsync(string phoneNumber,string code);
        Task<bool> ValidateVerificationCodeAsync(string phoneNumber,string code);
    }
}
