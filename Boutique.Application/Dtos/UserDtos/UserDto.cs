using AutoMapper.Configuration.Annotations;
using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos {
    public class UserDto {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool Blocked { get; set; } = false;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Gender { get; set; }
        public string? Age { get; set; }
        public string Role { get; set; }
        public List<Address>? Addresses { get; set; }
    }
}
