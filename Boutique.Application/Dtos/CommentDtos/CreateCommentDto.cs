using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos {
    public class CreateCommentDto {
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public IFormFile Image { get; set; } // Yorum resmi
    }
}
