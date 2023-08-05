using Infrastructure.Services;


namespace Tests.Services;

public class SessionOperationServiceTests
{
    [Fact]
    public async Task GetSessionCancellationToken_ValidSessionId_ReturnsCancellationTokenSource()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cancellationTokenSource = new CancellationTokenSource();

        var sessionService = new SessionOperationService();
        await sessionService.AddSession(sessionId, cancellationTokenSource);

        // Act
        var result = await sessionService.GetSessionCancellationToken(sessionId);

        // Assert
        Assert.Equal(cancellationTokenSource, result);
    }

    [Fact]
    public async Task GetSessionCancellationToken_InvalidSessionId_ThrowsArgumentException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var invalidSessionId = Guid.NewGuid();

        var cancellationTokenSource = new CancellationTokenSource();

        var sessionService = new SessionOperationService();
        await sessionService.AddSession(sessionId, cancellationTokenSource);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => sessionService.GetSessionCancellationToken(invalidSessionId));
    }

    [Fact]
    public async Task AddSession_NewSession_ReturnsTrue()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cancellationTokenSource = new CancellationTokenSource();

        var sessionService = new SessionOperationService();

        // Act
        var result = await sessionService.AddSession(sessionId, cancellationTokenSource);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddSession_ExistingSession_ReturnsFalse()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cancellationTokenSource = new CancellationTokenSource();

        var sessionService = new SessionOperationService();
        await sessionService.AddSession(sessionId, cancellationTokenSource);

        // Act
        var result = await sessionService.AddSession(sessionId, cancellationTokenSource);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EndOperationSession_ValidSessionId_ReturnsTrueAndCancelsCancellationToken()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cancellationTokenSource = Substitute.For<CancellationTokenSource>();

        var sessionService = new SessionOperationService();
        await sessionService.AddSession(sessionId, cancellationTokenSource); 

        // Act
        var result = await sessionService.EndOperationSession(sessionId);

        // Assert
        Assert.True(result);
        cancellationTokenSource.Received(1).Cancel();
    }

    [Fact]
    public async Task EndOperationSession_InvalidSessionId_ReturnsFalse()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var invalidSessionId = Guid.NewGuid();
        var cancellationTokenSource = new CancellationTokenSource();

        var sessionService = new SessionOperationService();
        await sessionService.AddSession(sessionId, cancellationTokenSource);

        // Act
        var result = await sessionService.EndOperationSession(invalidSessionId);

        // Assert
        Assert.False(result);
    }
}

