using AspMessengerPlus.Models;

namespace AspMessengerPlus.Services;

public class EchoChatService : IChatService
{
    public Task ConnectAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<ChatMessage> SendAsync(string text)
    {
        await Task.Delay(250);

        return new ChatMessage
        {
            Text = $"Echo: {text}",
            IsMine = false,
            Timestamp = DateTime.Now
        };
    }
}
