using Core.Domain;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

namespace API.GraphQL
{
    public class PakketType : ObjectType<Pakket>
    {
        protected override void Configure(IObjectTypeDescriptor<Pakket> descriptor)
        {
            descriptor.Field(p=>p.Id).Type<IdType>().Name("Id");
            descriptor.Field(p=>p.titel).Type<StringType>().Name("Titel");
            descriptor.Field(p => p.kantine).Type<KantineType>().Name("Kantine");
            descriptor.Field(p => p.pickup).Type<DateTimeType>().Name("Pickup");
            descriptor.Field(p => p.pickUpMax).Type<DateTimeType>().Name("PickUpMax");
            descriptor.Field(p => p.volwassen).Type<BooleanType>().Name("Volwassen");
            descriptor.Field(p => p.prijs).Type<DecimalType>().Name("Prijs");
            descriptor.Field(p => p.gereserveerd).Type<StudentType>().Name("Reserveerder");
        }
    }
}
