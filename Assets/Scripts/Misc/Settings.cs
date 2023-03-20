using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttampts = 10;
    #endregion DUNGEON BUILD SETTINGS

    #region ROOM SETTINGS
    // Max number of child corridors leading from a room.
    public const int maxChildCorridors = 3;
    #endregion
}
