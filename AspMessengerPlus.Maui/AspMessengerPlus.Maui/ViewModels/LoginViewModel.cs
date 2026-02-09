using System.Windows.Input;
using AspMessengerPlus.Maui.Services;
using Microsoft.Maui.Storage;

namespace AspMessengerPlus.Maui.ViewModels;

public class LoginViewModel : BindableObject
{
    private readonly IAuthService _authService;

    private string _email = "";
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    private string _password = "";
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        LoginCommand = new Command(async () => await Login());
    }

    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "Email and password required",
                "OK");
            return;
        }

        var user = await _authService.LoginAsync(Email, Password);

        if (user == null)
        {
            await Shell.Current.DisplayAlert(
                "Login failed",
                "Invalid credentials",
                "OK");
            return;
        }

        Preferences.Set("user_id", user.UserId);
        Preferences.Set("username", user.Username);

        // ✅ 官方推荐的 MAUI 导航方式
        await Shell.Current.GoToAsync("ChatPage");
    }
}
