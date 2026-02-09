using AspMessengerPlus.Maui.ViewModels;
using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ChatPage : ContentPage
{
    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

