using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ChatPage : ContentPage
{
    private readonly ChatViewModel _viewModel;
    private readonly long _channelId;
    private bool _initialized;

    public ChatPage(ChatViewModel viewModel, long channelId)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _channelId = channelId;

        BindingContext = _viewModel;

        _viewModel.Messages.CollectionChanged += (s, e) =>
        {
            if (_viewModel.Messages.Count == 0) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MessagesView.ScrollTo(
                    _viewModel.Messages.Count - 1,
                    position: ScrollToPosition.End,
                    animate: true);
            });
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_initialized) return;
        _initialized = true;

        await _viewModel.SwitchChannelAsync(_channelId);
    }
}
