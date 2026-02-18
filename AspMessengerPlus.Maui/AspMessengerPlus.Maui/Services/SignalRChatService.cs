using Microsoft.AspNetCore.SignalR.Client;
using AspMessengerPlus.Models;
using System.Net.Http;

namespace AspMessengerPlus.Services;

public class SignalRChatService : IChatService
{
    private HubConnection? _connection;

    private const string HubUrl = "https://10.0.2.2:7175/chatHub?channelId=47";

    public event Action<ChatMessage>? MessageReceived;

    public async Task ConnectAsync()
    {
        if (_connection != null)
            return;

#if ANDROID
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _connection = new HubConnectionBuilder()
            .WithUrl(HubUrl, options =>
            {
                options.HttpMessageHandlerFactory = _ => handler;
            })
            .WithAutomaticReconnect()
            .Build();
#else
        _connection = new HubConnectionBuilder()
            .WithUrl(HubUrl)
            .WithAutomaticReconnect()
            .Build();
#endif

        // 👇 修正：把 userName 赋值给 SenderName
        _connection.On<long, string, string, string, DateTime>(
            "ReceiveMessage",
            (id, userId, userName, message, createdAt) =>
            {
                MessageReceived?.Invoke(new ChatMessage
                {
                    Id = id.ToString(),
                    Text = message,
                    IsMine = false,
                    SenderName = string.IsNullOrWhiteSpace(userName)
                        ? "Unknown"
                        : userName,
                    Avatar = "tx2.jpg", // 对方头像
                    Timestamp = createdAt
                });
            });

        await _connection.StartAsync();
    }

    public async Task<ChatMessage> SendAsync(string text)
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            await ConnectAsync();
        }

        await _connection!.SendAsync("SendMessage", 47, text);

        // 👇 本地立即显示自己的消息
        return new ChatMessage
        {
            Id = Guid.NewGuid().ToString(),
            Text = text,
            IsMine = true,
            SenderName = "Me",
            Avatar = "tx.jpg", // 自己头像
            Timestamp = DateTime.Now,
            IsRead = false
        };
    }
}
