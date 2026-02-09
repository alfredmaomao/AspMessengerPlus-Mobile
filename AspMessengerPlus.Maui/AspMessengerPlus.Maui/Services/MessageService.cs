using AspMessengerPlus.Maui.Models;

namespace AspMessengerPlus.Maui.Services;

public class MessageService
{
    public async Task<Message> GetReplyAsync(string userText)
    {
      
        await Task.Delay(1000);

        userText = userText.ToLower();

        if (userText.Contains("hi") || userText.Contains("hello"))
        {
            return new Message
            {
                Text = "Hi 👋 Nice to meet you!",
                IsMine = false
            };
        }

        if (userText.Contains("how are"))
        {
            return new Message
            {
                Text = "I'm good 😊 How about you?",
                IsMine = false
            };
        }

        if (userText.Contains("bye"))
        {
            return new Message
            {
                Text = "Bye 👋 See you next time!",
                IsMine = false
            };
        }

        return new Message
        {
            Text = "🤖 I received: " + userText,
            IsMine = false
        };
    }
}
