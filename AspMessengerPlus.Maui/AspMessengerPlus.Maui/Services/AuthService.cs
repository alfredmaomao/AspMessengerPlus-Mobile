using System.Net.Http.Json;
using AspMessengerPlus.Maui.Models;

namespace AspMessengerPlus.Maui.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<UserDto?> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync(
            "api/auth/login",
            new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}
