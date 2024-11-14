using FlipCardProject.Data;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Services;
using System.Collections.Concurrent;

public class UserTrackingService
{
    private readonly ConcurrentDictionary<int, bool> _activePlayers;

    public UserTrackingService()
    {
        _activePlayers = new ConcurrentDictionary<int, bool>();
    }

    public void AddPlayer(int userId)
    {
        _activePlayers.TryAdd(userId, true);
    }

    public void RemovePlayer(int userId)
    {
        _activePlayers.TryRemove(userId, out _);
    }

    public int GetCurrentPlayerCount()
    {
        return _activePlayers.Count;
    }

    public List<int> GetActivePlayers()
    {
        return _activePlayers.Keys.ToList();
    }
}