using AspMessengerPlus.Maui.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
