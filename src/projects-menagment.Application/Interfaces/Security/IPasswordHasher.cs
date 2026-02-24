namespace projects_menagment.Application.Interfaces.Security;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
