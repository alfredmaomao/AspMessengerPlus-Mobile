using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AspMessengerPlus.Models;

public class ChatMessage : INotifyPropertyChanged
{
    private bool _isRead;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Text { get; set; } = "";
    public bool IsMine { get; set; }
    public string SenderName { get; set; } = "";
    public string Avatar { get; set; } = "";
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public bool IsRead
    {
        get => _isRead;
        set
        {
            if (_isRead == value) return;
            _isRead = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReadStatus));
        }
    }
    public bool IsTyping { get; set; }

    public string FormattedTime => Timestamp.ToString("HH:mm");

    public string ReadStatus =>
        IsMine ? (IsRead ? "✓✓" : "✓") : "";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
