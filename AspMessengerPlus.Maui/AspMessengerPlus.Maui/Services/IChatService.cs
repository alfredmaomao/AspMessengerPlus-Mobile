using AspMessengerPlus.Models;

namespace AspMessengerPlus.Services;

public interface IChatService
{
    Task ConnectAsync();
    Task<ChatMessage> SendAsync(string text);
}
