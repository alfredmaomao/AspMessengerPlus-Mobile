using AspMessengerPlus.Models;

namespace AspMessengerPlus.Services;

public interface IChatService
{
    Task<ChatMessage> SendAsync(string text);
}
