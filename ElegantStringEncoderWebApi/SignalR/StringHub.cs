using ElegantStringEncoderWebApi.Services;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace ElegantStringEncoderWebApi.SignalR;

[SignalRHub]
public class StringHub : Hub
{
    private readonly IStringEncoder _stringEncoder;
    private readonly ILogger<StringHub> _logger;

    public StringHub(IStringEncoder stringEncoder, ILogger<StringHub> logger)
    {
        _stringEncoder = stringEncoder;
        _logger = logger;
    }

    public async Task ConvertToBase64String(string input)
    {
        var encodedSymbols = _stringEncoder.GetBase64StringAsync(input);
        await foreach (var symbol in encodedSymbols)
        {
            await Clients.Caller.SendAsync("ConvertToBase64StringResponse", symbol);
        }
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("New connection with id: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Disconnect with id: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}