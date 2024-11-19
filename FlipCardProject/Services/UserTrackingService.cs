using FlipCardProject.Data;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Services;
using System.Collections.Concurrent;

public class UserTrackingService<T>
{
    private readonly ConcurrentDictionary<T, bool> _activePlayers;

    public UserTrackingService()
    {
        _activePlayers = new ConcurrentDictionary<T, bool>();
    }

    public void AddPlayer(T userId)
    {
        _activePlayers.TryAdd(userId, true);
    }

    public void RemovePlayer(T userId)
    {
        _activePlayers.TryRemove(userId, out _);
    }

    public int GetCurrentPlayerCount()
    {
        return _activePlayers.Count;
    }

    public List<T> GetActivePlayers()
    {
        return _activePlayers.Keys.ToList();
    }
}