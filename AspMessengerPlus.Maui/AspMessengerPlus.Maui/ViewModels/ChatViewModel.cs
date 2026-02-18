using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AspMessengerPlus.Models;
using AspMessengerPlus.Services;

namespace AspMessengerPlus.ViewModels;

public class ChatViewModel : INotifyPropertyChanged
{
    private readonly SignalRChatService _chatService;

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
        _chatService = (SignalRChatService)chatService;

        SendCommand = new Command(
            async () => await SendAsync(),
            () => !IsBusy && !string.IsNullOrWhiteSpace(InputText)
        );

        _chatService.MessageReceived += msg =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // 🔥 如果是我刚发的那条广播回来 → 忽略
                if (_lastSentMessage != null && msg.Text == _lastSentMessage)
                {
                    _lastSentMessage = null;
                    return; // 🚀 阻止重复
                }

                msg.IsMine = false;

                if (string.IsNullOrWhiteSpace(msg.SenderName))
                    msg.SenderName = "User";

                msg.Avatar = "tx2.jpg";

                Messages.Add(msg);
            });
        };

        Task.Run(async () => await _chatService.ConnectAsync());
    }

    private async Task SendAsync()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;

        try
        {
            IsBusy = true;

            _lastSentMessage = text;

            // ✅ 本地立即显示（不会重复）
            var localMessage = new ChatMessage
            {
                Text = text,
                SenderName = "Me",
                IsMine = true,
                Avatar = "tx.jpg",
                IsRead = false
            };

            Messages.Add(localMessage);

            await _chatService.SendAsync(text);

            InputText = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task SwitchChannelAsync(long newChannelId)
    {
        Messages.Clear();
        await _chatService.SwitchChannelAsync(newChannelId);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
