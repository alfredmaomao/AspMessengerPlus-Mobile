using AspMessengerPlus.Maui;
using AspMessengerPlus.Maui.Services;
using AspMessengerPlus.Maui.ViewModels;
using AspMessengerPlus.Services;
using AspMessengerPlus.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using System.Net.Http;

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

        // =========================
        // HttpClient（Android 不炸版）
        // =========================
        builder.Services.AddSingleton(sp =>
        {
#if ANDROID
            // Android：忽略 HTTPS 自签名证书（开发期必需）
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.0.2.2:7175/")
            };
#else
            // Windows / 其他平台
            return new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7175/")
            };
#endif
        });

        // =========================
        // Services
        // =========================
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IChatService, FakeChatService>();

        // =========================
        // ViewModels
        // =========================
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ChatViewModel>();

        // =========================
        // Pages
        // =========================
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ChatPage>();

        return builder.Build();
    }
}
