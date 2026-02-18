using AspMessengerPlus.Models;
using AspMessengerPlus.Services;

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

    private async void OnChannelSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ChannelDto channel)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ChatPage)}?channelId={channel.Id}");
        }
    }
}
