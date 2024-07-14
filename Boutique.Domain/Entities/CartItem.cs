using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Boutique.Domain.Entities.Product;

namespace Boutique.Domain.Entities {
    public class CartItem {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("unitPrice")]
        public decimal UnitPrice { get; set; }

        [BsonElement("totalPrice")]
        public decimal TotalPrice {
            get {
                return Quantity * UnitPrice;
            }
            private set { }
        }

        [BsonElement("variant")]
        public ProductVariant Variant { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
