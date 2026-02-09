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

    private string _statusText = string.Empty;
    public string StatusText
    {
        get => _statusText;
        set
        {
            if (_statusText == value) return;
            _statusText = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasStatus));
        }
    }

    public bool HasStatus => !string.IsNullOrWhiteSpace(StatusText);

    public ICommand SendCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ChatViewModel(IChatService chatService)
    {
        _chatService = chatService;

        SendCommand = new Command(
            execute: async () => await SendAsync(),
            canExecute: () => !IsBusy && !string.IsNullOrWhiteSpace(InputText)
        );

        Messages.Add(new ChatMessage
        {
            Text = "ChatViewModel connected ✅",
            IsMine = false,
            Timestamp = DateTime.Now
        });
    }

    private async Task SendAsync()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;

        try
        {
            IsBusy = true;
            StatusText = string.Empty;

          
            Messages.Add(new ChatMessage
            {
                Text = text,
                IsMine = true,
                Timestamp = DateTime.Now
            });

            InputText = string.Empty;

            
            var incoming = await _chatService.SendAsync(text);
            Messages.Add(incoming);
        }
        catch (Exception ex)
        {
            StatusText = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
