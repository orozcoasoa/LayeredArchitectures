using AutoMapper;
using CartingService.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.Core.BLL
{
    public class CartingProfile : Profile
    {
        public CartingProfile()
        {
            CreateMap<CartDAO, Cart>();
            CreateMap<Cart, CartDAO>();
            CreateMap<ItemDAO, Item>();
            CreateMap<Item, ItemDAO>();
        }
    }
}
