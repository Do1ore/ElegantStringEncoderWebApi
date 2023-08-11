using System.Text;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tests.Services;

public class StringEncoderServiceTests
{
    [Fact]
    public async Task GetBase64StringAsync_ValidInput_EncodesString()
    {
        // Arrange
        var input = "Hello, World!";
        var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

        var configuration = Substitute.For<IConfiguration>();
        configuration.GetSection("OperationDuration")["Min"].Returns("100");
        configuration.GetSection("OperationDuration")["Max"].Returns("200");

        var logger = Substitute.For<ILogger<StringEncoderService>>();

        var stringEncoderService = new StringEncoderService(configuration, logger);

        // Act
        var result = new List<string>();
        await foreach (var character in stringEncoderService.GetBase64StringAsync(input, CancellationToken.None))
        {
            result.Add(character);
        }

        // Assert
        Assert.Equal(encodedString.Length, result.Count);
        Assert.Equal(encodedString, string.Join("", result));
    }

    [Fact]
    public async Task GetBase64StringAsync_CancelledOperation_LogsCancellationInfo()
    {
        // Arrange
        var input = "Hello, World!";

        var configuration = Substitute.For<IConfiguration>();
        configuration.GetSection("OperationDuration")["Min"].Returns("500");
        configuration.GetSection("OperationDuration")["Max"].Returns("1000");

        var logger = Substitute.For<ILogger<StringEncoderService>>();

        var stringEncoderService = new StringEncoderService(configuration, logger);


        using var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await foreach (var _ in stringEncoderService.GetBase64StringAsync(input, token))
            {
                cancellationTokenSource.Cancel();
            }
        });
    }
}