using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AspMessengerPlus.Models;
using AspMessengerPlus.Services;
using Microsoft.Maui.Dispatching;

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

    // 🔥 用来记录刚刚自己发的消息
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
                    // 🔥 如果服务器广播的是刚刚自己发的那条
                    if (_lastSentMessage != null && msg.Text == _lastSentMessage)
                    {
                        msg.IsMine = true;
                        _lastSentMessage = null;
                    }

                    Messages.Add(msg);
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

            // 🔥 记录刚发送的内容
            _lastSentMessage = text;

            // 🔥 只发送，不本地添加
            await _chatService.SendAsync(text);

            InputText = string.Empty;
        }
        catch (Exception ex)
        {
            Messages.Add(new ChatMessage
            {
                Text = $"Error: {ex.Message}",
                IsMine = false,
                Timestamp = DateTime.Now
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
