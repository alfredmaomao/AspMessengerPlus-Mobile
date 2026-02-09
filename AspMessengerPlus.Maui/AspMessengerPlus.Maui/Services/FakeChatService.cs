using AspMessengerPlus.Models;
using AspMessengerPlus.Services;

namespace AspMessengerPlus.Maui.Services;

public class FakeChatService : IChatService
{
    public async Task<ChatMessage> SendAsync(string text)
    {
        // 模拟网络延迟
        await Task.Delay(600);

        return new ChatMessage
        {
            Text = $"🤖 Echo: {text}",
            IsMine = false,
            Timestamp = DateTime.Now
        };
    }
}
