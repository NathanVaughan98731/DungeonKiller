using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region UNITS
    public const float pixelsPerUnit = 16f;
    public const float tileSizePIxels = 16f;
    #endregion

    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttampts = 10;
    #endregion DUNGEON BUILD SETTINGS

    #region ROOM SETTINGS
    // Max number of child corridors leading from a room.
    public const int maxChildCorridors = 3;
    public const float fadeInTime = 0.5f; // Time to fade in the room
    public const float doorUnluckDelay = 1f;
    #endregion


    #region ANIMATOR PARAMETERS
    // Animator parameters - Player
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollDown = Animator.StringToHash("rollDown");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static float baseSpeedForPlayerAnimations = 8f;

    // Animator parameters - Enemy
    public static float baseSpeedForEnemyAnimations = 3f;

    // Animator parameters - Door
    public static int open = Animator.StringToHash("open");

    #endregion

    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion

    #region FIRING CONTROL
    public const float useAimAngleDistance = 3.5f;
    #endregion

    #region A* PATHFINDING PARAMETERS
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const int targetFrameRateToSpreadPathfindingOver = 60;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;
    #endregion

    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region UI PARAMETERS
    public const float uiHeartSpacing = 16f;
    public const float uiAmmoIconSpacing = 4f;
    #endregion
}
