using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ChatPage : ContentPage
{
    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.Messages.CollectionChanged += (s, e) =>
        {
            if (viewModel.Messages.Count == 0)
                return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MessagesView.ScrollTo(
                    viewModel.Messages.Count - 1,
                    position: ScrollToPosition.End,
                    animate: true);
            });
        };
    }
}
