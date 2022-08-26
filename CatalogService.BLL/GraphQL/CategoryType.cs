using CatalogService.BLL.Entities;

namespace CatalogService.BLL.GraphQL
{
    public class CategoryType : ObjectType<Category>
    {
        protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(f => f.Id);
            descriptor.Field(f => f.Name)
                .Type<StringType>();
            descriptor.Field(f => f.ParentCategory)
                .Type<CategoryType>();
        }
    }
}
