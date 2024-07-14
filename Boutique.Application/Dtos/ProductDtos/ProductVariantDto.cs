using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.ProductDtos {
    public class ProductVariantDto {
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("stockQuantity")]
        public int StockQuantity { get; set; }
    }
}
