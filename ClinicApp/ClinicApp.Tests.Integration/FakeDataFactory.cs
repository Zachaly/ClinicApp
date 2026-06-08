using Bogus;
using ClinicApp.Domain.Request;

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
}
