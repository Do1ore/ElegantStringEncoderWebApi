namespace Infrastructure.Abstractions;

/// <summary>
/// Service to manage operation sessions
/// </summary>
public interface ISessionOperationService
{
    Task<bool> AddSession(Guid sessionId, CancellationTokenSource cancellationTokenSource);
    Task<bool> EndOperationSession(Guid sessionId);
}