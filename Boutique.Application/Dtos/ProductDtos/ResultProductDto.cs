using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boutique.Domain.Entities.Product;

namespace Boutique.Application.Dtos.ProductDtos {
    public class ResultProductDto {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public List<ProductVariant> Variants { get; set; }
        public List<string> ImageUrls { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public decimal NewPrice { get; set; }
        public List<string>? ExistingImageUrls { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public string ProductCode { get; set; }
    }
}
