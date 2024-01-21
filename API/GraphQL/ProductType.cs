using Core.Domain;

namespace API.GraphQL
{
    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(p => p.Id).Type<IdType>().Name("Id");
            descriptor.Field(p => p.naam).Type<StringType>().Name("Naam");
            descriptor.Field(p => p.alcohol).Type<BooleanType>().Name("Alcohol");
            descriptor.Field(p => p.foto).Type<StringType>().Name("Foto");
        }
    }
}
