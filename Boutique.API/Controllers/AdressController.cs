using Boutique.API.Models;
using Boutique.Application.Services.AdressServices;
using Boutique.Domain.Entities;
using Iyzipay.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController:ControllerBase {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService) {
            _addressService = addressService;
        }

        [Authorize]
        [HttpGet("get-user-address/{userId}")]
        public async Task<IActionResult> GetUserAddresses(string userId) {
            var addresses = await _addressService.GetUserAddressesAsync(userId);
            return Ok(addresses);
        }

        [Authorize]
        [HttpGet("get-address/{id:guid}")]
        public async Task<IActionResult> GetAddressById(Guid id) {
            var address = await _addressService.GetAddressByIdAsync(id);
            if(address == null) {
                return NotFound();
            }
            return Ok(address);
        }

        [Authorize]
        [HttpPost("create-address")]
        public async Task<IActionResult> CreateAddress([FromBody] Domain.Entities.Address address) {
            address.Id = Guid.NewGuid();
            await _addressService.CreateAddressAsync(address);
            return Ok(new BaseResponse() { Message = "Address registered successfully",Result = address,Success = true });
        }

        [Authorize]
        [HttpPut("update-address{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id,[FromBody] Domain.Entities.Address address) {
            if(id != address.Id) {
                return BadRequest();
            }

            await _addressService.UpdateAddressAsync(address);
            return Ok(new BaseResponse() { Message = "Address updated successfully",Result = address,Success = true });
        }

        [Authorize]
        [HttpDelete("delete-address{id:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid id) {
            await _addressService.DeleteAddressAsync(id);
            return Ok(new BaseResponse() { Message = "Address deleted successfully",Success = true });
        }
    }
}
