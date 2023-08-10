namespace Infrastructure.Abstractions;


/// <summary>
/// Service to operate long-runnind string encodings
/// </summary>
public interface IStringEncoderService
{
    IAsyncEnumerable<string> GetBase64StringAsync(string input, CancellationToken cancellationToken);
}