using AspMessengerPlus.Maui.Models;

namespace AspMessengerPlus.Maui.Services;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(string email, string password);

    Task<bool> RegisterAsync(string email, string password);
}
