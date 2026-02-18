using AspMessengerPlus.ViewModels;

namespace AspMessengerPlus.Maui;

public partial class ConversationListPage : ContentPage
{
    public ConversationListPage()
    {
        InitializeComponent();
    }

    private async void OnChannel49Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new ChatPage(
            App.Current.Handler.MauiContext.Services.GetService<ChatViewModel>()));
    }

    private async void OnChannel50Clicked(object sender, EventArgs e)
    {
        await Shell.Current.DisplayAlert("Channel", "50 clicked", "OK");
    }

    private async void OnChannel88Clicked(object sender, EventArgs e)
    {
        await Shell.Current.DisplayAlert("Channel", "88 clicked", "OK");
    }
}
