using Boutique.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Domain.Entities {
    public class Comment {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentId { get; set; }

        [BsonElement("produtcId")]
        public string ProductId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("likes")]
        public int Likes { get; set; }

        [BsonElement("replies")]
        public List<CommentReply> Replies { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
  
}
