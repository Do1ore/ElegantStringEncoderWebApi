using System.Collections.Concurrent;
using Infrastructure.Abstractions;

namespace Infrastructure.Services;

/// <summary>
/// Service to manage operation sessions. Implementation of
/// <seealso cref="ISessionOperationService"/>
/// </summary>
public class SessionOperationService : ISessionOperationService
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _sessions = new();

    /// <summary>
    /// Get cancellation token from ConcurrentDictionary by id
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>CancellationTokenSource</returns>
    /// <exception cref="ArgumentException"></exception>
    public Task<CancellationTokenSource> GetSessionCancellationToken(Guid sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var cancellationTokenSource))
        {
            return Task.FromResult(cancellationTokenSource);
        }

        throw new ArgumentException("Session with this id not found");
    }

    /// <summary>
    /// Add session to ConcurrentDictionary by id
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationTokenSource"></param>
    /// <returns>Task of bool</returns>
    public Task<bool> AddSession(Guid sessionId, CancellationTokenSource cancellationTokenSource)
    {
        if (_sessions.TryAdd(sessionId, cancellationTokenSource))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Remove session from ConcurrentDictionary
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>Task of bool</returns>
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