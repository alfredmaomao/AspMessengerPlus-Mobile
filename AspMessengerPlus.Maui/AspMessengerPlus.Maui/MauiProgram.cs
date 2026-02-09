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

        // =========================
        // ✅ HttpClient（非常关键）
        // =========================
        builder.Services.AddSingleton(new HttpClient
        {
            // 🔴 Windows / Android Emulator 用这个
            //BaseAddress = new Uri("https://10.0.2.2:7175/")

            // 👉 如果你现在只在 Windows 跑，可以临时用：
            BaseAddress = new Uri("https://localhost:7175/")
        });

        // =========================
        // ✅ Services
        // =========================
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IChatService, EchoChatService>();

        // =========================
        // ✅ ViewModels
        // =========================
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ChatViewModel>();

        // =========================
        // ✅ Pages
        // =========================
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ChatPage>();

        return builder.Build();
    }
}
