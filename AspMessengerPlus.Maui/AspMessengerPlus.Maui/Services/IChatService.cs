using AspMessengerPlus.Models;

namespace AspMessengerPlus.Services;

public interface IChatService
{
    event Action<ChatMessage> MessageReceived;

    Task ConnectAsync();
    Task<ChatMessage> SendAsync(string text);
    Task SwitchChannelAsync(long newChannelId);
}
