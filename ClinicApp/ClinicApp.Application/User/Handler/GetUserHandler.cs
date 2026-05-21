using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.User.Handler;

public class GetUserHandler
{
    private readonly IUserRepository _repository;
    private readonly UserModelMapper _mapper;

    public GetUserHandler(IUserRepository userRepository)
    {
        _repository = userRepository;
        _mapper = new UserModelMapper();
    }

    public async Task<List<UserModel>> Handle(GetUserRequest request)
    {
        var users = await _repository.GetAsync(request, ["Claims"]);

        return users.Select(_mapper.MapApplicationUserToModel).ToList();
    }
}
