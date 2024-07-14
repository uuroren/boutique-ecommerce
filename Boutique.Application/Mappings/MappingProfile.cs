using AutoMapper;
using Boutique.Application.Dtos;
using Boutique.Application.Dtos.CartDtos;
using Boutique.Application.Dtos.CategoryDtos;
using Boutique.Application.Dtos.CommentDtos;
using Boutique.Application.Dtos.FavoriteDtos;
using Boutique.Application.Dtos.OrderDtos;
using Boutique.Application.Dtos.ProductDtos;
using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boutique.Domain.Entities.Product;

namespace Boutique.Application.Mappings {
    public class MappingProfile:Profile {
        public MappingProfile() {
            CreateMap<User,UserDto>().ReverseMap();

            CreateMap<Address,AddressDto>().ReverseMap();

            CreateMap<Comment,CreateCommentDto>().ReverseMap();
            CreateMap<CommentReply,CreateCommentReplyDto>().ReverseMap();


            CreateMap<Category,CreateCategoryDto>().ReverseMap();
            CreateMap<Category,UpdateCategoryDto>().ReverseMap();
            CreateMap<Category,ResultCategoryDto>().ReverseMap();

            CreateMap<CreateProductDto,Product>()
                .ForMember(dest => dest.Variants,opt => opt.Ignore()).ForMember(dest => dest.Tags,opt => opt.Ignore()).ReverseMap();
            CreateMap<UpdateProductDto,Product>()
                .ForMember(dest => dest.Variants,opt => opt.Ignore()).ForMember(dest => dest.Tags,opt => opt.Ignore()).ReverseMap();
            CreateMap<ProductVariantDto,ProductVariant>().ReverseMap();
            CreateMap<Product,ResultProductDto>().ReverseMap();
            CreateMap<ProductVariant,ProductVariantDto>().ReverseMap();

            CreateMap<CreateCartDto,Cart>().ReverseMap();
            CreateMap<Cart,CartDto>().ReverseMap();
            CreateMap<CartItemDto,CartItem>().ReverseMap();
            CreateMap<CartItem,CartItemDto>().ReverseMap();

            CreateMap<Order,OrderDTO>().ReverseMap();
            CreateMap<OrderItem,OrderItemDTO>().ReverseMap();

            CreateMap<Favorite,CreateFavoriteDto>().ReverseMap();
        }
    }
}
