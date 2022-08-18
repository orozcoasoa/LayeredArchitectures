using CatalogService.BLL.Entities;

namespace CatalogService.BLL.GraphQL
{
    public class ItemInputType : InputObjectType<ItemDTO>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ItemDTO> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Name("ItemInput");
            descriptor.Field(f => f.Name);
            descriptor.Field(f => f.Description);
            descriptor.Field(f => f.CategoryId);
            descriptor.Field(f => f.Price);
            descriptor.Field(f => f.Amount);
        }
    }
}
