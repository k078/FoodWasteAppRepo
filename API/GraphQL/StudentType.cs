using Core.Domain;

namespace API.GraphQL
{
    public class StudentType : ObjectType<Student>
    {
        protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
        {
            descriptor.Field(s => s.Id).Type<IdType>().Name("Id");
            descriptor.Field(s => s.naam).Type<StringType>().Name("Naam");
            descriptor.Field(s => s.studentnummer).Type<IntType>().Name("Studentnr");
            descriptor.Field(s => s.email).Type<StringType>().Name("Email");
            descriptor.Field(s => s.geboortedatum).Type<DateTimeType>().Name("Geboortedatum");
        }
    }
}
