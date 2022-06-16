using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Core.BLL
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<DAL.Category, Category>();
            CreateMap<Category, DAL.Category>()
                .ForMember(c => c.ParentCategory, opt => opt.Ignore());
            CreateMap<DAL.Item, Item>();
            CreateMap<Item, DAL.Item>()
                .ForMember(i => i.Category, opt => opt.Ignore());
        }
    }
}
