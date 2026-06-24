namespace MyBookApi2.Services;

public interface IAuthService
{
    bool ValidateCredentials(string username, string password, out string role);
    string GenerateToken(string username, string role);
}
