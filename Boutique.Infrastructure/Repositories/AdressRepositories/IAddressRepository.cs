using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.AdressRepositories {
    public interface IAddressRepository {
        Task<IEnumerable<Address>> GetUserAddressesAsync(string userId);
        Task<Address> GetUserDefaultAdressAsync(string userId);
        Task<Address> GetAddressByIdAsync(Guid id);
        Task CreateAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Guid id);
    }
}
