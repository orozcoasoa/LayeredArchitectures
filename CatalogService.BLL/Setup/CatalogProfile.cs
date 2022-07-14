using AutoMapper;
using CatalogService.BLL.Entities;

namespace CatalogService.BLL.Setup
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<DAL.Category, Category>();
            CreateMap<Category, DAL.Category>()
                .ForMember(c => c.ParentCategory, opt => opt.Ignore());
            CreateMap<CategoryDTO, Category>()
                .ForMember(c => c.ParentCategory,
                        opt => opt.MapFrom((d) => d.ParentCategoryId == null ? null : new Category() { Id = (int)d.ParentCategoryId }));

            CreateMap<DAL.Item, Item>();
            CreateMap<Item, DAL.Item>()
                .ForMember(i => i.Category, opt => opt.Ignore());
            CreateMap<ItemDTO, Item>()
                .ForMember(c => c.Category,
                        opt => opt.MapFrom((d) => new Category() { Id = d.CategoryId }));

            CreateMap<DAL.Item, MessagingService.Contracts.Item>();
        }
    }
}
