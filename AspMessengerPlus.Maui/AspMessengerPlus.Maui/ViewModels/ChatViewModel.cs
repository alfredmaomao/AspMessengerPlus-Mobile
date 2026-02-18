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
    private readonly MessageService _messageService;

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

            if (!string.IsNullOrWhiteSpace(value))
                _ = _chatService.SendTypingAsync();
        }
    }

    private string _typingText = "Online";
    public string TypingText
    {
        get => _typingText;
        set
        {
            if (_typingText == value) return;
            _typingText = value;
            OnPropertyChanged();
        }
    }

    public ICommand SendCommand { get; }
    public event PropertyChangedEventHandler? PropertyChanged;

    private ChatMessage? _typingBubble;

    // ⭐ 用于防止回声
    private string? _lastSentText;
    private DateTime _lastSentTime;

    public ChatViewModel(
        SignalRChatService chatService,
        MessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;

        SendCommand = new Command(
            async () => await SendAsync(),
            () => !string.IsNullOrWhiteSpace(InputText)
        );

        _chatService.MessageReceived += msg =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RemoveTypingBubble();

                // ⭐ 防回声逻辑
                if (_lastSentText == msg.Text &&
                    (DateTime.UtcNow - _lastSentTime).TotalSeconds < 3)
                {
                    return;
                }

                Messages.Add(msg);
            });
        };

        _chatService.UserTypingReceived += userName =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                ShowTypingBubble(userName);
                await Task.Delay(1500);
                RemoveTypingBubble();
            });
        };
    }

    private void ShowTypingBubble(string userName)
    {
        RemoveTypingBubble();

        TypingText = $"{userName} is typing...";

        _typingBubble = new ChatMessage
        {
            IsTyping = true,
            IsMine = false,
            Avatar = "tx2.jpg"
        };

        Messages.Add(_typingBubble);
    }

    private void RemoveTypingBubble()
    {
        if (_typingBubble != null)
        {
            Messages.Remove(_typingBubble);
            _typingBubble = null;
        }

        TypingText = "Online";
    }

    private async Task SendAsync()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;

        // 记录发送内容和时间（用于防回声）
        _lastSentText = text;
        _lastSentTime = DateTime.UtcNow;

        // 本地立即显示
        Messages.Add(new ChatMessage
        {
            Text = text,
            IsMine = true,
            Avatar = "tx.jpg"
        });

        await _chatService.SendAsync(text);

        InputText = string.Empty;
    }

    public async Task SwitchChannelAsync(long newChannelId)
    {
        Messages.Clear();

        var history = await _messageService.GetMessagesAsync(newChannelId);

        foreach (var msg in history)
            Messages.Add(msg);

        await _chatService.SwitchChannelAsync(newChannelId);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
