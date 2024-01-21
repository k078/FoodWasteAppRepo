using Core.Domain;

namespace API.GraphQL
{
    public class KantineType : ObjectType<Kantine>
    {
        protected override void Configure(IObjectTypeDescriptor<Kantine> descriptor)
        {
            descriptor.Field(k => k.Id).Type<IdType>().Name("Id");
            descriptor.Field(k => k.naam).Type<StringType>().Name("Naam");
        }
    }
}
