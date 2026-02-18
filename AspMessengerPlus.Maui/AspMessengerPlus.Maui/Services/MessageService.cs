using System.Net.Http.Json;
using Microsoft.Maui.Storage;
using AspMessengerPlus.Models;

namespace AspMessengerPlus.Services;

public class MessageService
{
    private readonly HttpClient _http;

    public MessageService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ChatMessage>> GetMessagesAsync(long channelId)
    {
        var result = await _http.GetFromJsonAsync<List<MessageResponse>>(
            $"/api/messages/{channelId}");

        if (result == null)
            return new List<ChatMessage>();

        var currentUserId = Preferences.Get("user_id", "");

        return result.Select(m => new ChatMessage
        {
            Id = m.id.ToString(),
            Text = m.text,
            IsMine = m.userId == currentUserId, // 🔥 关键修复
            SenderName = $"{m.firstName} {m.lastName}",
            Avatar = m.userId == currentUserId ? "tx.jpg" : "tx2.jpg",
            Timestamp = m.createdAt
        }).ToList();
    }

    private class MessageResponse
    {
        public long id { get; set; }
        public string text { get; set; } = "";
        public string userId { get; set; } = "";
        public DateTime createdAt { get; set; }
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
    }
}
