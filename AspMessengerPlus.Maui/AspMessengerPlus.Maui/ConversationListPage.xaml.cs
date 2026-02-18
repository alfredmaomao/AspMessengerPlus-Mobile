using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ConversationListPage : ContentPage
{
    private readonly ChatViewModel _viewModel;

    public ConversationListPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private async void OnChannel49Clicked(object sender, EventArgs e)
    {
        await NavigateToChannel(49);
    }

    private async void OnChannel27Clicked(object sender, EventArgs e)
    {
        await NavigateToChannel(27);
    }

    private async void OnChannel88Clicked(object sender, EventArgs e)
    {
        await NavigateToChannel(88);
    }

    private async Task NavigateToChannel(long channelId)
    {
        await _viewModel.SwitchChannelAsync(channelId);

        await Shell.Current.Navigation.PushAsync(
            new ChatPage(_viewModel));
    }
}
