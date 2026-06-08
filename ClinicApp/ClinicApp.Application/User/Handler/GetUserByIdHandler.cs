using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.User.Handler;

public record GetUserByIdRequest(Guid Id);

public class GetUserByIdHandler
{
    private readonly IUserRepository _repository;
    private readonly UserModelMapper _mapper;

    public GetUserByIdHandler(IUserRepository repository)
    {
        _repository = repository;
        _mapper = new UserModelMapper();
    }

    public async Task<UserModel?> Handle(GetUserByIdRequest request)
    {
        var user = await _repository.GetByIdAsync(request.Id, ["Claims"]);

        return user is null ? null : _mapper.MapApplicationUserToModel(user);
    }
}
