using SportingStatsBackEnd.Models;

namespace SportingStatsBackEnd.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string emailOrUsername, string password);
        string GenerateJwtToken(User user);
        Task<bool> UserExistsAsync(string email, string username);
        Task<User> RegisterAsync(string email, string username, string password);
    }
}
