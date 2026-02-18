using AspMessengerPlus.Maui.Models;
using AspMessengerPlus.Services;
using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class NewChatPage : ContentPage
{
    private readonly ChannelService _channelService;

    public NewChatPage(ChannelService channelService)
    {
        InitializeComponent();
        _channelService = channelService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var users = await _channelService.GetUsersAsync();
        UsersView.ItemsSource = users;
    }

    private async void OnUserTapped(object sender, EventArgs e)
    {
        if (sender is not Border border)
            return;

        if (border.BindingContext is not UserDto user)
            return;

        // 🔥 创建私聊频道
        var channelId = await _channelService.CreatePrivateChannelAsync(user.UserId);

        if (channelId == 0)
        {
            await DisplayAlert("Error", "Failed to create chat", "OK");
            return;
        }

        // 🔥 获取 ChatViewModel
        var services = Application.Current!.Handler!.MauiContext!.Services;
        var vm = services.GetRequiredService<ChatViewModel>();

        // 🔥 切换频道
        await vm.SwitchChannelAsync(channelId);

        var chatPage = new ChatPage(vm, channelId);

        await Navigation.PushAsync(chatPage);
    }
}
