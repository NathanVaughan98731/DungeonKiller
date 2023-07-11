using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StaticEventHandler
{
    // Room changed event
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs() { room = room });
    }

    // Room enemies have been defeated event
    public static event Action<RoomEnemiesDefeatedArgs> OnRoomEnemiesDefeated;

    public static void CallRoomEnemiesDefeatedEvent(Room room)
    {
        OnRoomEnemiesDefeated?.Invoke(new RoomEnemiesDefeatedArgs() { room = room });
    }

    // Points scored event
    public static event Action<PointsScoredArgs> OnPointsScored;

    public static void CallPointsScoredEvent(int points)
    {
        OnPointsScored?.Invoke(new PointsScoredArgs() { points = points });
    }

    // Score changed event
    public static event Action<ScoreChangedArgs> OnScoreChanged; 

    public static void CallScoreChangedEvent(long score)
    {
        OnScoreChanged?.Invoke(new ScoreChangedArgs() { score = score });
    }
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;
}

public class RoomEnemiesDefeatedArgs : EventArgs
{
    public Room room;
}

public class PointsScoredArgs : EventArgs 
{
    public int points;
}

public class ScoreChangedArgs : EventArgs
{
    public long score;
}
