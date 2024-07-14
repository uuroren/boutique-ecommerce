using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.FavoriteDtos {
    public class ResultFavoriteDto {
        public string UserId { get; set; }
        public Product Product { get; set; }
    }
}
