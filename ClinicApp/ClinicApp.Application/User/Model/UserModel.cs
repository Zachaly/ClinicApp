namespace ClinicApp.Application.User.Model;

public class UserModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Claims { get; set; }
}
