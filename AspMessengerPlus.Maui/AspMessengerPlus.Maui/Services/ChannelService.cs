using AspMessengerPlus.Maui.Models;
using AspMessengerPlus.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace AspMessengerPlus.Services;

public class ChannelService
{
    private readonly HttpClient _httpClient;

    public ChannelService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ChannelDto>> GetChannelsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ChannelDto>>("/api/channels");
        return result ?? new List<ChannelDto>();
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<UserDto>>("/api/users");
        return result ?? new List<UserDto>();
    }

    public async Task<int> CreatePrivateChannelAsync(string otherUserId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/channels/private",
            new { OtherUserId = otherUserId });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("CreatePrivateChannel failed: " + error);
            return 0;
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<CreateChannelResponse>(json, options);

        return result?.ChannelId ?? 0;
    }
}

public class CreateChannelResponse
{
    public int ChannelId { get; set; }
}
