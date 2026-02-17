using Microsoft.AspNetCore.SignalR.Client;
using AspMessengerPlus.Models;
using System.Net.Http;

namespace AspMessengerPlus.Services;

public class SignalRChatService : IChatService
{
    private HubConnection? _connection;

    private const string HubUrl = "https://10.0.2.2:7175/chatHub?channelId=47";

    // 🔥 这里写死当前登录用户（你现在用 PY）
    private const string CurrentUserId = "PY";

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

        _connection.On<long, string, string, string, DateTime>(
            "ReceiveMessage",
            (id, userId, userName, message, createdAt) =>
            {
                MessageReceived?.Invoke(new ChatMessage
                {
                    Text = message,
                    // 🔥 关键判断
                    IsMine = userId == CurrentUserId,
                    Timestamp = createdAt
                });
            });

        await _connection.StartAsync();

        Console.WriteLine("SignalR connected ✅");
    }

    public async Task<ChatMessage> SendAsync(string text)
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            await ConnectAsync();
        }

        await _connection!.SendAsync("SendMessage", 47, text);

        // 🔥 不再本地生成 UI 消息
        // 交给服务器广播回来处理

        return new ChatMessage
        {
            Text = text,
            IsMine = true,
            Timestamp = DateTime.Now
        };
    }
}
