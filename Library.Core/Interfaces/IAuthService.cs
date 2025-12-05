using Library.Core.Models;

namespace Library.Core.Interfaces
{
public interface IAuthService
{
Task<User> LoginAsync(string username, string password);
Task LogoutAsync(int userId);
Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
Task<User> GetUserByIdAsync(int userId);
Task<bool> ValidateUserAsync(string username, string password);

}
}