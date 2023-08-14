namespace Infrastructure.Abstractions;


/// <summary>
/// Service to operate long-running string encodings
/// </summary>
public interface IStringEncoderService
{
    IAsyncEnumerable<string> GetBase64StringAsync(string input, CancellationToken cancellationToken);
    int Base64StringSymbolsCount(string input);
}