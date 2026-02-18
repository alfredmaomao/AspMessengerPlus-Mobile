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

        try
        {
            var channels = await _channelService.GetChannelsAsync();
            ChannelsView.ItemsSource = channels;
        }
        catch
        {
            await DisplayAlert("Error", "Failed to load conversations.", "OK");
        }
    }

    private async void OnChannelTapped(object sender, EventArgs e)
    {
        if (sender is not Frame frame)
            return;

        if (frame.BindingContext is not ChannelDto channel)
            return;

        var services = Application.Current?.Handler?.MauiContext?.Services;
        if (services == null)
            return;

        var vm = services.GetRequiredService<ChatViewModel>();
        var chatPage = new ChatPage(vm, channel.Id);

        await Navigation.PushAsync(chatPage);
    }

    private async void OnNewChatClicked(object sender, EventArgs e)
    {
        var services = Application.Current?.Handler?.MauiContext?.Services;
        if (services == null)
            return;

        var page = services.GetRequiredService<NewChatPage>();
        await Navigation.PushAsync(page);
    }
}
