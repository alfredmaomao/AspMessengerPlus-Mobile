using Microsoft.AspNetCore.SignalR.Client;
using AspMessengerPlus.Models;
using System.Net;

namespace AspMessengerPlus.Services;

public class SignalRChatService : IChatService
{
    private HubConnection? _connection;
    private readonly CookieContainer _cookieContainer;

    private long _currentChannelId = 49;

#if ANDROID
    private const string BaseHubUrl = "https://10.0.2.2:7175/chatHub";
#else
    private const string BaseHubUrl = "https://localhost:7175/chatHub";
#endif

    public event Action<ChatMessage>? MessageReceived;

    public SignalRChatService(CookieContainer cookieContainer)
    {
        _cookieContainer = cookieContainer;
    }

    // ===============================
    // 连接
    // ===============================
    public async Task ConnectAsync()
    {
        if (_connection != null &&
            _connection.State == HubConnectionState.Connected)
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

        var hubUrl = $"{BaseHubUrl}?channelId={_currentChannelId}";

        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.HttpMessageHandlerFactory = _ => handler;
            })
            .WithAutomaticReconnect()
            .Build();

        // 🔥 确保不会重复注册事件
        _connection.Remove("ReceiveMessage");

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

    // ===============================
    // 发送消息
    // ===============================
    public async Task<ChatMessage> SendAsync(string text)
    {
        if (_connection == null ||
            _connection.State != HubConnectionState.Connected)
        {
            await ConnectAsync();
        }

        await _connection!.SendAsync("SendMessage", _currentChannelId, text);

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

    // ===============================
    // 切换频道
    // ===============================
    public async Task SwitchChannelAsync(long newChannelId)
    {
        if (_currentChannelId == newChannelId)
            return;

        _currentChannelId = newChannelId;

        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }

        await ConnectAsync();
    }
}
