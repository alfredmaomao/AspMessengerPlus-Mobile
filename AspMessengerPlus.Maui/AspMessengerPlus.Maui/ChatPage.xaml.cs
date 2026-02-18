using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

[QueryProperty(nameof(ChannelId), "channelId")]
public partial class ChatPage : ContentPage
{
    private readonly ChatViewModel _viewModel;

    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        _viewModel.Messages.CollectionChanged += (s, e) =>
        {
            if (_viewModel.Messages.Count == 0)
                return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MessagesView.ScrollTo(
                    _viewModel.Messages.Count - 1,
                    position: ScrollToPosition.End,
                    animate: true);
            });
        };
    }

    public string ChannelId
    {
        set
        {
            if (long.TryParse(value, out var id))
            {
                _viewModel.SwitchChannelAsync(id);
            }
        }
    }
}
