namespace ElegantStringEncoderWebApi.Services;

public interface IStringEncoder
{
    IAsyncEnumerable<string> GetBase64StringAsync(string input);
}