using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Domain.Entities {
    public class CommentReply {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommentReplyId { get; set; }

        [BsonElement("commentId")]
        public string CommentId { get; set; }
        [BsonElement("userId")]
        public string UserId { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
