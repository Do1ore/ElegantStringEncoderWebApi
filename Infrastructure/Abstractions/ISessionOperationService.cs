using System.Collections.Concurrent;

namespace Infrastructure.Abstractions;

public interface ISessionOperationService
{
    Task<CancellationTokenSource> GetSessionCancellationToken(Guid sessionId);
    Task<bool> AddSession(Guid sessionId, CancellationTokenSource cancellationTokenSource);
    Task<bool> EndOperationSession(Guid sessionId);
}