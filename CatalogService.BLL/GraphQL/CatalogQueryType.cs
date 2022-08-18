namespace CatalogService.BLL.GraphQL
{
    public class CatalogQueryType : ObjectType<CatalogQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogQuery> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
