using System.Runtime.CompilerServices;
using System.Text;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <summary>
/// Service to operate long-running string encodings
/// </summary>
public class StringEncoderService : IStringEncoderService
{

    private readonly int _minOperationDuration;
    private readonly int _maxOperationDuration;
    private readonly ILogger<StringEncoderService> _logger;

    public StringEncoderService(IConfiguration configuration, ILogger<StringEncoderService> logger)
    {
        _logger = logger;
        _minOperationDuration = Convert.ToInt32(configuration.GetSection("OperationDuration")["Min"]
                                                ?? throw new ApplicationException("OperationDuration:min not found."));
        _maxOperationDuration = Convert.ToInt32(configuration.GetSection("OperationDuration")["Max"]
                                                ?? throw new ApplicationException("OperationDuration:max not found."));
    }

    public int Base64StringSymbolsCount(string input)
    {
        var byteArray = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(byteArray).Length;
    }

    /// <summary>
    /// Long-running async conversion from string to base64String
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>IAsyncEnumerable of string </returns>
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