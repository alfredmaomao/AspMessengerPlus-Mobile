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

#if ANDROID
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookies,
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.0.2.2:7175/")
            };
#else
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookies
            };

            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7175/")
            };
#endif
        });

        builder.Services.AddSingleton<IAuthService, AuthService>();

        // SignalR chat
        builder.Services.AddSingleton<SignalRChatService>();
        builder.Services.AddSingleton<IChatService>(sp => sp.GetRequiredService<SignalRChatService>());

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddSingleton<ChatViewModel>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ConversationListPage>();

        // Services
        builder.Services.AddSingleton<ChannelService>();

        return builder.Build();
    }
}
