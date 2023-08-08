using Infrastructure.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Application.SignalR;

[SignalRHub]
public class StringHub : Hub
{
    private readonly IStringEncoderService _stringEncoderService;
    private readonly ILogger<StringHub> _logger;
    private readonly ISessionOperationService _sessionOperationService;

    public StringHub(IStringEncoderService stringEncoderService, ILogger<StringHub> logger,
        ISessionOperationService sessionOperationService)
    {
        _stringEncoderService = stringEncoderService;
        _logger = logger;
        _sessionOperationService = sessionOperationService;
    }

    public async Task ConvertToBase64String(string input, string sessionId)
    {
        var cancellationToken = new CancellationTokenSource();
        await _sessionOperationService.AddSession(Guid.Parse(sessionId), cancellationToken);

        var encodedSymbols = _stringEncoderService
            .GetBase64StringAsync(input, cancellationToken.Token);

        await foreach (var symbol in encodedSymbols)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await Clients.Caller.SendAsync("ConvertToBase64StringResponse", symbol,
                cancellationToken: cancellationToken.Token);
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