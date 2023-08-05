namespace Infrastructure.Abstractions;

public interface IStringEncoderService
{
    IAsyncEnumerable<string> GetBase64StringAsync(string input, CancellationToken cancellationToken);
}