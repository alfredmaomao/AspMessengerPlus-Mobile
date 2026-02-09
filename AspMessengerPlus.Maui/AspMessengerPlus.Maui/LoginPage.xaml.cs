using AspMessengerPlus.Maui.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
