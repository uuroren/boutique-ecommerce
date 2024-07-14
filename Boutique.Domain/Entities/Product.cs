using Boutique.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Domain.Entities {
    public class Product {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("category_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonElement("product_variants")]
        public List<ProductVariant> Variants { get; set; } // Varyantlar (beden, renk vs.)

        [BsonElement("image_urls")]
        public List<string> ImageUrls { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("published")]
        public bool IsPublished { get; set; }

        [BsonElement("new_price")]
        public decimal NewPrice { get; set; }

        [BsonElement("discount_percentage")]
        public decimal DiscountPercentage { get; set; }

        [BsonElement("product_code")]
        public string ProductCode { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        // Default constructor
        public Product() {
            Variants = new List<ProductVariant>();
            ImageUrls = new List<string>();
            Tags = new List<string>();
        }

        public class ProductVariant {
            public string Size { get; set; }
            public string Color { get; set; }
            public int StockQuantity { get; set; }
        }
    }
}
