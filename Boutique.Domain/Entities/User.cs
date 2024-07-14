using Boutique.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Domain.Entities {
    public class User:IdentityUser<Guid> {
        public bool Blocked { get; set; } = false;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Gender { get; set; }
        public string? Age { get; set; }
        public RoleType Role { get; set; }
        public string IdentityNumber { get; set; }
    }
}
