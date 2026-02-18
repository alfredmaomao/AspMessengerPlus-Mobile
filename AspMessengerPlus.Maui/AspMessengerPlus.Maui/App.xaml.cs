namespace AspMessengerPlus.Maui;

public partial class App : Application
{
    public App(LoginPage loginPage)
    {
        InitializeComponent();

        var nav = new NavigationPage(loginPage)
        {
            BarBackgroundColor = Colors.Transparent,
            BarTextColor = Colors.White
        };

        NavigationPage.SetHasNavigationBar(loginPage, false);

        MainPage = nav;
    }
}
