using AspMessengerPlus.Models;
using AspMessengerPlus.Services;
using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ConversationListPage : ContentPage
{
    private readonly ChannelService _channelService;

    public ConversationListPage(ChannelService channelService)
    {
        InitializeComponent();
        _channelService = channelService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var channels = await _channelService.GetChannelsAsync();
        ChannelsView.ItemsSource = channels;
    }

    private async void OnChannelTapped(object sender, EventArgs e)
    {
        if (sender is not Frame frame)
            return;

        if (frame.BindingContext is not ChannelDto channel)
            return;

        var services = Application.Current!.Handler!.MauiContext!.Services;
        var vm = services.GetRequiredService<ChatViewModel>();

        var chatPage = new ChatPage(vm, channel.Id);

        await Navigation.PushAsync(chatPage);
    }
}
