using System.Net.Http.Json;
using AspMessengerPlus.Models;

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
}
