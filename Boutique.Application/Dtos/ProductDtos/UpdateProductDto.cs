using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.ProductDtos {
    public class UpdateProductDto {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal NewPrice { get; set; }
        public string CategoryId { get; set; }
        public string Variants { get; set; }
        public List<string>? ExistingImageUrls { get; set; } 
        public List<IFormFile>? NewImages { get; set; }
        public int Stock { get; set; }
        public bool IsPublished { get; set; }
    }
}
