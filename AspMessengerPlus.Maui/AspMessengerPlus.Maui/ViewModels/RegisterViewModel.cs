using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AspMessengerPlus.Maui.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
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

    public RegisterViewModel()
    {
        RegisterCommand = new Command(async () =>
        {
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                Password != ConfirmPassword)
            {
                await Application.Current!.MainPage!
                    .DisplayAlert("Error", "Invalid input", "OK");
                return;
            }

            await Application.Current!.MainPage!
                .DisplayAlert("Success", "Account created!", "OK");
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
