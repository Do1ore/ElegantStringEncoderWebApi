using System.Collections.Concurrent;
using Infrastructure.Abstractions;

namespace Infrastructure.Services;

/// <summary>
/// Service to manage operation sessions. Implementation of
/// <seealso cref="ISessionOperationService"/>
/// </summary>
public class SessionOperationService : ISessionOperationService
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _sessions;

    public SessionOperationService(ConcurrentDictionary<Guid, CancellationTokenSource> sessions)
    {
        _sessions = sessions;
    }
    
    /// <summary>
    /// Add session to ConcurrentDictionary by id
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationTokenSource"></param>
    /// <returns>Task of bool</returns>
    public Task<bool> AddSession(Guid sessionId, CancellationTokenSource cancellationTokenSource)
    {
        return Task.FromResult(_sessions.TryAdd(sessionId, cancellationTokenSource));
    }

    /// <summary>
    /// Remove session from ConcurrentDictionary
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>Task of bool</returns>
    public Task<bool> EndOperationSession(Guid sessionId)
    {
        if (!_sessions.TryRemove(sessionId, out var cancellationTokenSource))
            return Task.FromResult(false);

        cancellationTokenSource.Cancel();
        return Task.FromResult(true);
    }
}