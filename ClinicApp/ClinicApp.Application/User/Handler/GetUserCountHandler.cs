using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.User.Handler;

public class GetUserCountRequest : GetUserRequest;

public class GetUserCountHandler
{
    private readonly IUserRepository _repository;

    public GetUserCountHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public Task<int> Handle(GetUserCountRequest request)
        => _repository.GetCountAsync(request);
}
