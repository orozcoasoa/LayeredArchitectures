using CatalogService.BLL.Entities;

namespace CatalogService.BLL.GraphQL
{
    public class CategoryInputType : InputObjectType<CategoryDTO>
    {

        protected override void Configure(IInputObjectTypeDescriptor<CategoryDTO> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Name("CategoryInput");
            descriptor.Field(f => f.Name);
            descriptor.Field(f => f.ParentCategoryId)
                .Type<NonNullType<IntType>>();
        }
    }
}
