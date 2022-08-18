using CatalogService.BLL.Entities;
using CatalogService.DAL;

namespace CatalogService.BLL.GraphQL
{
    public class CatalogMutationType : ObjectType<CatalogMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogMutation> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field("addCategory")
                .Argument("category", a => a.Type<CategoryInputType>())
                .Type<CategoryType>()
                .Resolve(async context =>
                {
                    var category = context.ArgumentValue<CategoryDTO>("category");
                    var catalog = context.Service<ICatalogService>();
                    var newCat = await catalog.AddCategory(category);
                    return await catalog.GetCategory(newCat.Id);
                })
                .Error<ArgumentException>();

            descriptor.Field("updateCategory")
                .Argument("category", a => a.Type<CategoryInputType>())
                .Argument("id", a => a.Type<IntType>())
                .Type<CategoryType>()
                .Resolve(async context =>
                {
                    var category = context.ArgumentValue<CategoryDTO>("category");
                    var categoryId = context.ArgumentValue<int>("id");
                    var catalog = context.Service<ICatalogService>();
                    await catalog.UpdateCategory(categoryId, category);
                    return await catalog.GetCategory(categoryId);
                })
                .Error<ArgumentException>()
                .Error<KeyNotFoundException>();

            descriptor.Field("deleteCategory")
                .Argument("id", a => a.Type<IntType>())
                .Type<BooleanType>()
                .Resolve(async context =>
                {
                    var categoryId = context.ArgumentValue<int>("id");
                    var catalog = context.Service<ICatalogService>();
                    await catalog.DeleteCategory(categoryId);
                    return true;
                });

            descriptor.Field("addItem")
                .Argument("item", a => a.Type<ItemInputType>())
                .Type<ItemType>()
                .Resolve(async context =>
                {
                    var item = context.ArgumentValue<ItemDTO>("item");
                    var catalog = context.Service<ICatalogService>();
                    return await catalog.AddItem(item);
                })
                .Error<ArgumentException>();

            descriptor.Field("updateItem")
                .Argument("item", a => a.Type<ItemInputType>())
                .Argument("id", a => a.Type<IntType>())
                .Type<ItemType>()
                .Resolve(async context =>
                {
                    var item = context.ArgumentValue<ItemDTO>("item");
                    var id = context.ArgumentValue<int>("id");
                    var catalog = context.Service<ICatalogService>();
                    await catalog.UpdateItem(id, item);
                    return await catalog.GetItem(id);
                })
                .Error<ArgumentException>()
                .Error<KeyNotFoundException>();

            descriptor.Field("deleteItem")
                .Argument("id", a => a.Type<IntType>())
                .Type<BooleanType>()
                .Resolve(async context =>
                {
                    var id = context.ArgumentValue<int>("id");
                    var catalog = context.Service<ICatalogService>();
                    await catalog.DeleteItem(id);
                    return true;
                })
                .Error<ArgumentException>()
                .Error<KeyNotFoundException>();

        }
    }
}
