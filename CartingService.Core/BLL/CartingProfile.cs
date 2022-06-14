using AutoMapper;
using CartingService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.BLL
{
    public class CartingProfile : Profile
    {
        public CartingProfile()
        {
            CreateMap<CartDAO, Cart>();
            CreateMap<ItemDAO, Item>();
            CreateMap<Item, ItemDAO>();
        }
    }
}
