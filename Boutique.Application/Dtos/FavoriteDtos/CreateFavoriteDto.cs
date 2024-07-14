using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.FavoriteDtos {
    public class CreateFavoriteDto {
        public string UserId { get; set; }
        public string ProductId { get; set; }
    }
}
