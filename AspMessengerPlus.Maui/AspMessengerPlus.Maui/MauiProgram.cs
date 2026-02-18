using AspMessengerPlus.Maui;
using AspMessengerPlus.Maui.Services;
using AspMessengerPlus.Maui.ViewModels;
using AspMessengerPlus.Services;
using AspMessengerPlus.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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

        // 🔥 全局唯一 CookieContainer
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
        builder.Services.AddSingleton<IChatService, SignalRChatService>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ChatViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ChatPage>();

        return builder.Build();
    }
}
