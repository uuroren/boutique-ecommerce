using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.AdressServices {
    public interface IAddressService {
        Task<IEnumerable<Address>> GetUserAddressesAsync(string userId);
        Task<Address> GetAddressByIdAsync(Guid id);
        Task CreateAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Guid id);
    }
}
