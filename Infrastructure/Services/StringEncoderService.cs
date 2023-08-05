using System.Runtime.CompilerServices;
using System.Text;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class StringEncoderService : IStringEncoderService
{
    private readonly ISessionOperationService _sessionService;

    private readonly int _minOperationDuration;
    private readonly int _maxOperationDuration;
    private readonly ILogger<StringEncoderService> _logger;

    public StringEncoderService(ISessionOperationService sessionService,
        IConfiguration configuration, ILogger<StringEncoderService> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
        _minOperationDuration = Convert.ToInt32(configuration.GetSection("OperationDuration")["Min"]
                                                ?? throw new ApplicationException("OperationDuration:min not found."));
        _maxOperationDuration = Convert.ToInt32(configuration.GetSection("OperationDuration")["Max"]
                                                ?? throw new ApplicationException("OperationDuration:max not found."));
    }

    public async IAsyncEnumerable<string> GetBase64StringAsync(string input,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var byteArray = Encoding.UTF8.GetBytes(input);
        var encodedString = Convert.ToBase64String(byteArray);


        foreach (var character in encodedString)
        {
            try
            {
                await Task.Delay(Random.Shared.Next(_minOperationDuration, _maxOperationDuration + 1),
                    cancellationToken);
            }
            catch (TaskCanceledException canceledException)
            {
                _logger.LogInformation("Operation canceled successfully: {CancellationInfo}",
                    canceledException.Message);
                throw;
            }

            yield return character.ToString();
        }
    }
}