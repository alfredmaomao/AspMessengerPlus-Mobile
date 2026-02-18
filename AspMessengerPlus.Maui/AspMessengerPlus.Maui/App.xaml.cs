namespace AspMessengerPlus.Maui;

public partial class App : Application
{
    public App(LoginPage loginPage)
    {
        InitializeComponent();

        var nav = new NavigationPage(loginPage)
        {
            BarBackgroundColor = Color.FromArgb("#0B1220"),
            BarTextColor = Colors.White
        };

        MainPage = nav;
    }
}
