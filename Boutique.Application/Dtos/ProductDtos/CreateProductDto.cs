using Boutique.Application.Dtos.ProductDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos {
    public class CreateProductDto {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
        public string Variants { get; set; }
        public string Tags { get; set; }
        public bool IsPublished { get; set; }
        public int Stock { get; set; }

    }
}
