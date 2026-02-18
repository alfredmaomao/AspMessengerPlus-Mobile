using Microsoft.AspNetCore.SignalR.Client;
using AspMessengerPlus.Models;
using System.Net;

namespace AspMessengerPlus.Services;

public class SignalRChatService : IChatService
{
    private HubConnection? _connection;
    private readonly CookieContainer _cookieContainer;

#if ANDROID
    private const string HubUrl = "https://10.0.2.2:7175/chatHub?channelId=49";
#else
    private const string HubUrl = "https://localhost:7175/chatHub?channelId=49";
#endif

    public event Action<ChatMessage>? MessageReceived;

    public SignalRChatService(CookieContainer cookieContainer)
    {
        _cookieContainer = cookieContainer;
    }

    public async Task ConnectAsync()
    {
        if (_connection != null)
            return;

#if ANDROID
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = _cookieContainer,
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
#else
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = _cookieContainer
        };
#endif

        _connection = new HubConnectionBuilder()
            .WithUrl(HubUrl, options =>
            {
                options.HttpMessageHandlerFactory = _ => handler;
            })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<long, string, string, string, DateTime>(
            "ReceiveMessage",
            (id, userId, userName, message, createdAt) =>
            {
                MessageReceived?.Invoke(new ChatMessage
                {
                    Id = id.ToString(),
                    Text = message,
                    IsMine = false,
                    SenderName = userName,
                    Avatar = "tx2.jpg",
                    Timestamp = createdAt
                });
            });

        await _connection.StartAsync();
    }

    public async Task<ChatMessage> SendAsync(string text)
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
            await ConnectAsync();

        await _connection!.SendAsync("SendMessage", 49, text);

        return new ChatMessage
        {
            Id = Guid.NewGuid().ToString(),
            Text = text,
            IsMine = true,
            SenderName = "Me",
            Avatar = "tx.jpg",
            Timestamp = DateTime.Now,
            IsRead = false
        };
    }
}
