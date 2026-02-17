using Microsoft.AspNetCore.SignalR.Client;
using AspMessengerPlus.Models;
using System.Net.Http;

namespace AspMessengerPlus.Services;

public class SignalRChatService : IChatService
{
    private HubConnection? _connection;

    //private const string HubUrl = "https://10.0.2.2:7175/chatHub?channelId=27";
    private const string HubUrl =
    "https://aspmessengerplus-cgccdravd4c2hjb8.canadacentral-01.azurewebsites.net/chatHub?channelId=27";

    public event Action<ChatMessage>? MessageReceived;

    public async Task ConnectAsync()
    {
        if (_connection != null)
            return;

        //#if ANDROID
        //        var handler = new HttpClientHandler
        //        {
        //            ServerCertificateCustomValidationCallback =
        //                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        //        };

        //        _connection = new HubConnectionBuilder()
        //            .WithUrl(HubUrl, options =>
        //            {
        //                options.HttpMessageHandlerFactory = _ => handler;
        //            })
        //            .WithAutomaticReconnect()
        //            .Build();
        //#else
        //        _connection = new HubConnectionBuilder()
        //            .WithUrl(HubUrl)
        //            .WithAutomaticReconnect()
        //            .Build();
        //#endif

        _connection = new HubConnectionBuilder()
    .WithUrl(HubUrl)
    .WithAutomaticReconnect()
    .Build();


        _connection.On<long, string, string, string, DateTime>(
            "ReceiveMessage",
            (id, userId, userName, message, createdAt) =>
            {
                MessageReceived?.Invoke(new ChatMessage
                {
                    Text = message,
                    IsMine = false,   // 服务器广播统一灰色
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

        await _connection!.SendAsync("SendMessage", 27, text);

        // 本地立即显示蓝色气泡
        return new ChatMessage
        {
            Text = text,
            IsMine = true,
            Timestamp = DateTime.Now
        };
    }
}
