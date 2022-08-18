using CatalogService.BLL.Entities;

namespace CatalogService.BLL.GraphQL
{
    public class ItemType : ObjectType<Item>
    {
        protected override void Configure(IObjectTypeDescriptor<Item> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(f => f.Id);
            descriptor.Field(f => f.Name);
            descriptor.Field(f => f.Description);
            descriptor.Field(f => f.Category)
                .Type<CategoryType>();
            descriptor.Field(f => f.Price)
                .Type<DecimalType>();
            descriptor.Field(f => f.Amount);
        }
    }
}
