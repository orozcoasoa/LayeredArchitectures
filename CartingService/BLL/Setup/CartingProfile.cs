using AutoMapper;
using CartingService.DAL.Entities;

namespace CartingService.BLL.Setup
{
    public class CartingProfile : Profile
    {
        public CartingProfile()
        {
            //Cart mappings
            CreateMap<CartDAO, Cart>();
            CreateMap<Cart, CartDAO>();

            //Item mappings
            CreateMap<CartItemDAO, Item>()
                .ForMember(i => i.Id, opt => opt.MapFrom(d => d.Item.Id))
                .ForMember(i => i.Name, opt => opt.MapFrom(d => d.Item.Name))
                .ForMember(i => i.Image, opt => opt.MapFrom(d => d.Item.Image))
                .ForMember(i => i.Price, opt => opt.MapFrom(d => d.Item.Price));
            CreateMap<Item, CartItemDAO>()
                .ForMember(i => i.Id, opt => opt.Ignore());
            
            //Messaging mappings
            CreateMap<MessagingService.Contracts.Item, ItemDAO>();
        }
    }
}
