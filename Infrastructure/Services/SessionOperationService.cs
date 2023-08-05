using System.Collections.Concurrent;
using Infrastructure.Abstractions;

namespace Infrastructure.Services;

public class SessionOperationService : ISessionOperationService
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _sessions = new();


    public Task<CancellationTokenSource> GetSessionCancellationToken(Guid sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var cancellationTokenSource))
        {
            return Task.FromResult(cancellationTokenSource);
        }

        throw new ArgumentException("Session with this id not found");
    }

    public Task<bool> AddSession(Guid sessionId, CancellationTokenSource cancellationTokenSource)
    {
        if (_sessions.TryAdd(sessionId, cancellationTokenSource))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
    
    public Task<bool> EndOperationSession(Guid sessionId)
    {
        if (_sessions.TryRemove(sessionId, out var cancellationTokenSource))
        {
            cancellationTokenSource.Cancel();
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}