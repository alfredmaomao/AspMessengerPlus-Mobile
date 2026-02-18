using AspMessengerPlus.Maui.Services;
using AspMessengerPlus.Services;
using Microsoft.Maui.Storage;
using System.Windows.Input;

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
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                "Email and password required",
                "OK");
            return;
        }

        var user = await _authService.LoginAsync(Email, Password);

        if (user == null)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Login failed",
                "Invalid credentials",
                "OK");
            return;
        }

        Preferences.Set("user_id", user.UserId);
        Preferences.Set("username", user.Username);

        // ✅ 正确导航方式（不重新创建 MainPage）
        var services = Application.Current!.Handler!.MauiContext!.Services;
        var conversationPage = services.GetRequiredService<ConversationListPage>();

        await Application.Current!.MainPage!.Navigation.PushAsync(conversationPage);
    }
}
