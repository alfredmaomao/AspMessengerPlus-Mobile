using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AspMessengerPlus.Maui.Services;

namespace AspMessengerPlus.Maui.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
        RegisterCommand = new Command(async () => await RegisterAsync());
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    private string _confirmPassword = string.Empty;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set { _confirmPassword = value; OnPropertyChanged(); }
    }

    public ICommand RegisterCommand { get; }

    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            Password != ConfirmPassword)
        {
            await Application.Current!.MainPage!
                .DisplayAlert("Error", "Invalid input", "OK");
            return;
        }

        var success = await _authService.RegisterAsync(Email, Password);

        if (success)
        {
            await Application.Current!.MainPage!
                .DisplayAlert("Success", "Account created!", "OK");

            await Application.Current!.MainPage!.Navigation.PopAsync();
        }
        else
        {
            await Application.Current!.MainPage!
                .DisplayAlert("Error", "Registration failed", "OK");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
