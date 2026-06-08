using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using ClinicApp.Infrastructure.Repository.Base;

namespace ClinicApp.Infrastructure.Repository;

public class UserRepository : ReadRepositoryBase<ApplicationUser, GetUserRequest>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}
