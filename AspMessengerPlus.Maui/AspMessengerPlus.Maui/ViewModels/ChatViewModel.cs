using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AspMessengerPlus.Models;
using AspMessengerPlus.Services;

namespace AspMessengerPlus.ViewModels;

public class ChatViewModel : INotifyPropertyChanged
{
    private readonly IChatService _chatService;

    public ObservableCollection<ChatMessage> Messages { get; } = new();

    private string _inputText = string.Empty;
    public string InputText
    {
        get => _inputText;
        set
        {
            if (_inputText == value) return;
            _inputText = value;
            OnPropertyChanged();
            ((Command)SendCommand).ChangeCanExecute();
        }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy == value) return;
            _isBusy = value;
            OnPropertyChanged();
            ((Command)SendCommand).ChangeCanExecute();
        }
    }

    public ICommand SendCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private string? _lastSentMessage;

    public ChatViewModel(IChatService chatService)
    {
        _chatService = chatService;

        SendCommand = new Command(
            async () => await SendAsync(),
            () => !IsBusy && !string.IsNullOrWhiteSpace(InputText)
        );

        if (_chatService is SignalRChatService signalR)
        {
            signalR.MessageReceived += msg =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (_lastSentMessage != null && msg.Text == _lastSentMessage)
                    {
                        msg.IsMine = true;
                        msg.SenderName = "Me";
                        msg.Avatar = "tx.jpg";        // 👤 你的头像
                        msg.IsRead = false;
                        _lastSentMessage = null;
                    }
                    else
                    {
                        msg.IsMine = false;

                        // 保留服务器传来的名字
                        if (string.IsNullOrWhiteSpace(msg.SenderName))
                            msg.SenderName = "Unknown";

                        msg.Avatar = "tx2.jpg";
                    }


                    Messages.Add(msg);

                    if (msg.IsMine)
                        SimulateReadStatus(msg);
                });
            };
        }
    }

    private async Task SendAsync()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;

        try
        {
            IsBusy = true;

            _lastSentMessage = text;

            await _chatService.SendAsync(text);

            InputText = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void SimulateReadStatus(ChatMessage message)
    {
        await Task.Delay(1200);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            message.IsRead = true;
        });
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
