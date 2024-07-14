using Boutique.Domain.Entities;
using Boutique.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.AdressRepositories {
    public class AddressRepository:IAddressRepository {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(string userId) {
            return await _context.Addresses.AsNoTracking().Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(Guid id) {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task CreateAddressAsync(Address address) {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Address address) {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(Guid id) {
            var address = await _context.Addresses.FindAsync(id);
            if(address != null) {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Address> GetUserDefaultAdressAsync(string userId) {
            return await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }
    }
}

