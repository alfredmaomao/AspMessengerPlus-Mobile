namespace AspMessengerPlus.Maui;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.RegisterViewModel();
    }
}
