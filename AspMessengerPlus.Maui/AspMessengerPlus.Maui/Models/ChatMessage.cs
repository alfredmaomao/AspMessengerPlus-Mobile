namespace AspMessengerPlus.Models;

public class ChatMessage
{
    public string Text { get; set; } = string.Empty;
    public bool IsMine { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
