using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ChatPage : ContentPage
{
    public ChatPage(ChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
