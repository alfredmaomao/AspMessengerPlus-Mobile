using AspMessengerPlus.Maui.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        var services = Application.Current!.Handler!.MauiContext!.Services;
        var registerPage = services.GetRequiredService<RegisterPage>();

        await Application.Current!.MainPage!.Navigation.PushAsync(registerPage);
    }
}
