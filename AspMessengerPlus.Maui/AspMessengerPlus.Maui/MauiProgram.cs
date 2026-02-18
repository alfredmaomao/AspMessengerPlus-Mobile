using AspMessengerPlus.Maui;
using AspMessengerPlus.Maui.Services;
using AspMessengerPlus.Maui.ViewModels;
using AspMessengerPlus.Services;
using AspMessengerPlus.ViewModels;
using Microsoft.Maui.Hosting;
using System.Net;

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

        builder.Services.AddSingleton<CookieContainer>();

        builder.Services.AddSingleton(sp =>
        {
            var cookies = sp.GetRequiredService<CookieContainer>();

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookies
            };

            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://aspmessengerplus-cgccdravd4c2hjb8.canadacentral-01.azurewebsites.net/")
            };
        });

        builder.Services.AddSingleton<IAuthService, AuthService>();

        // SignalR chat
        builder.Services.AddSingleton<SignalRChatService>();
        builder.Services.AddSingleton<IChatService>(sp => sp.GetRequiredService<SignalRChatService>());

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddSingleton<ChatViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddSingleton<ChatViewModel>();
        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ConversationListPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ConversationListPage>();
        // Services
        builder.Services.AddSingleton<ChannelService>();
        builder.Services.AddSingleton<MessageService>();

        return builder.Build();
    }
}
