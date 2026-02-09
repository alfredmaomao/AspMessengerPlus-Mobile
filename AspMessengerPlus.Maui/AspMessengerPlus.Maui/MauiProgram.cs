using AspMessengerPlus.Maui;
using AspMessengerPlus.Maui.Models;
using AspMessengerPlus.Maui.Services;
using AspMessengerPlus.Maui.ViewModels;
using AspMessengerPlus.Services;
using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

                                                                         
        builder.Services.AddSingleton(new HttpClient
        {
            
            //BaseAddress = new Uri("https://10.0.2.2:7175/")

            
            BaseAddress = new Uri("https://localhost:7175/")
        });

       
        builder.Services.AddSingleton<IAuthService, AuthService>();
        // builder.Services.AddSingleton<IChatService, EchoChatService>();
        builder.Services.AddSingleton<IChatService, FakeChatService>();


        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ChatViewModel>();

   
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ChatPage>();

        return builder.Build();
    }
}
