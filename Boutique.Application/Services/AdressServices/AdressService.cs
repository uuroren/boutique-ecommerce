using Boutique.Domain.Entities;
using Boutique.Infrastructure.Repositories.AdressRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.AdressServices {
    public class AddressService:IAddressService {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository) {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(string userId) {
            return await _addressRepository.GetUserAddressesAsync(userId);
        }

        public async Task<Address> GetAddressByIdAsync(Guid id) {
            return await _addressRepository.GetAddressByIdAsync(id);
        }

        public async Task CreateAddressAsync(Address address) {
            if(address.IsDefault) {
                var existingDefaultAddress = await _addressRepository.GetUserDefaultAdressAsync(address.UserId);
                if(existingDefaultAddress != null) {
                    existingDefaultAddress.IsDefault = false;
                    await _addressRepository.UpdateAddressAsync(existingDefaultAddress);
                }
            }

            await _addressRepository.CreateAddressAsync(address);
        }

        public async Task UpdateAddressAsync(Address address) {
            if(address.IsDefault) {
                var existingDefaultAddress = await _addressRepository.GetUserDefaultAdressAsync(address.UserId);
                if(existingDefaultAddress != null && existingDefaultAddress.Id != address.Id) {
                    existingDefaultAddress.IsDefault = false;
                    await _addressRepository.UpdateAddressAsync(existingDefaultAddress);
                }
            }

            await _addressRepository.UpdateAddressAsync(address);
        }

        public async Task DeleteAddressAsync(Guid id) {
            await _addressRepository.DeleteAddressAsync(id);
        }
    }

}
