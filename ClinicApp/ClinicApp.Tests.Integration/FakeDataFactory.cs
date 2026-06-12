using Bogus;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using Bogus.Extensions.Poland;

namespace ClinicApp.Tests.Integration;

public static class FakeDataFactory
{
    public static List<CreateUserRequest> CreateUserRequests(int count)
        => new Faker<CreateUserRequest>()
            .RuleFor(r => r.Email, f => f.Internet.Email())
            .RuleFor(r => r.LastName, f => f.Name.LastName())
            .RuleFor(r => r.FirstName, f => f.Name.FirstName())
            .RuleFor(r => r.Password, _ => "Passw0rd!")
            .RuleFor(r => r.UserName, f => f.Random.AlphaNumeric(15))
            .Generate(count);

    public static List<Patient> CreatePatients(int count)
        => new Faker<Patient>()
            .RuleFor(r => r.Address, f => f.Address.StreetAddress())
            .RuleFor(r => r.City, f => f.Address.City())
            .RuleFor(r => r.BirthDate, f => f.Date.Past(10))
            .RuleFor(r => r.FirstName, f => f.Name.FirstName())
            .RuleFor(r => r.LastName, f => f.Name.LastName())
            .RuleFor(r => r.PeselNumber, f => f.Person.Pesel())
            .RuleFor(r => r.PostalCode, f => $"{f.Random.Number(10, 19)}-{f.Random.Number(100, 999)}")
            .Generate(count);

    public static List<DrugClass> CreateDrugClasses(int count)
        => new Faker<DrugClass>()
            .RuleFor(x => x.Name, f => f.Random.AlphaNumeric(20))
            .Generate(count);

    public static List<Drug> CreateDrugs(Guid classId, int count)
        => new Faker<Drug>()
            .RuleFor(x => x.ClassId, _ => classId)
            .RuleFor(x => x.BrandName, f => f.Random.AlphaNumeric(20))
            .RuleFor(x => x.GenericName, f => f.Random.AlphaNumeric(20))
            .Generate(count);
}
