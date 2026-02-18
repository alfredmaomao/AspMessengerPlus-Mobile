using Microsoft.AspNetCore.SignalR.Client;
using AspMessengerPlus.Models;
using System.Net;
using Microsoft.AspNetCore.Http.Connections;

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
    public event Action<string>? UserTypingReceived;

    public SignalRChatService(CookieContainer cookieContainer)
    {
        _cookieContainer = cookieContainer;
    }

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
                options.Transports = HttpTransportType.LongPolling;
                options.HttpMessageHandlerFactory = _ => handler;
            })
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

        _connection.On<string>("UserTyping", userName =>
        {
            UserTypingReceived?.Invoke(userName);
        });

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await _connection.StartAsync(cts.Token);
    }

    public async Task<ChatMessage> SendAsync(string text)
    {
        if (_connection == null ||
            _connection.State != HubConnectionState.Connected)
        {
            await ConnectAsync();
        }

        try
        {
            await _connection!.SendAsync("SendMessage", _currentChannelId, text);
        }
        catch { }

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

    public async Task SendTypingAsync()
    {
        if (_connection == null ||
            _connection.State != HubConnectionState.Connected)
            return;

        try
        {
            await _connection.SendAsync("Typing", _currentChannelId);
        }
        catch { }
    }

    public async Task SwitchChannelAsync(long newChannelId)
    {
        _currentChannelId = newChannelId;

        if (_connection != null)
        {
            try
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
            }
            catch { }

            _connection = null;
        }

        await ConnectAsync();
    }
}
