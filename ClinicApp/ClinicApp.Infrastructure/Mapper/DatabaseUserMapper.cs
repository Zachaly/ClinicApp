using ClinicApp.Database.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using Riok.Mapperly.Abstractions;

namespace ClinicApp.Infrastructure.Mapper;

[Mapper]
public partial class DatabaseUserMapper
{
    public partial DatabaseUser MapRequestToDatabaseModel(CreateUserRequest request);
    public partial ApplicationUser MapDatabaseModelToUser(DatabaseUser databaseUser);
    public partial DatabaseUser MapApplicationUserToDatabaseModel(ApplicationUser user);
}
