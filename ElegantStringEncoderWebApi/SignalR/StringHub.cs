using ElegantStringEncoderWebApi.Services;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace ElegantStringEncoderWebApi.SignalR;

[SignalRHub]
public sealed class StringHub : Hub
{
    private readonly IStringEncoder _stringEncoder;

    public StringHub(IStringEncoder stringEncoder)
    {
        _stringEncoder = stringEncoder;
    }

    public async Task Base64String(string input)
    {
        var encodedSymbols = _stringEncoder.GetBase64StringAsync(input);
        await foreach (var symbol in encodedSymbols)
        {
            await Clients.Caller.SendAsync("Base64StringResponce", symbol);
        }
    }

    public async Task Send(int arg1, string arg2, [SignalRHidden] CancellationToken ct = default)
    {
        await Clients.All.SendAsync("Receive", arg1.ToString(), arg2, ct);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}