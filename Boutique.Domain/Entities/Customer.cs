using Boutique.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Domain.Entities {
    public class Customer:BaseEntity {
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
