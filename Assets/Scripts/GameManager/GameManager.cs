using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region Header DUNGEON LEVELS

    [Space(10)]
    [Header("DUNGEON LEVELS")]

    #endregion Header DUNGEON LEVELS

    #region Tooltip

    [Tooltip("Populate with the dungeon level scriptable objects")]

    #endregion Tooltip

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("Populate with the starting dungeon level for testing, first level = 0")]
    #endregion Tooltip

    [SerializeField] private int currentDungeonLevelListIndex = 0;
    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long gameScore;
    private int scoreMultiplier;
    private InstantiatedRoom bossRoom;

    protected override void Awake()
    {
        // Call base class
        base.Awake();

        // Set player details - saved in current player scriptable object from the main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        // Instantiate player
        InstantiatePlayer();
    }

    // Create player in scene at position
    private void InstantiatePlayer()
    {
        // Instantiate player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        // Initialise player
        player = playerGameObject.GetComponent<Player>();

        player.Initialise(playerDetails);
    }

    private void OnEnable()
    {
        // Subscribe to room changed event
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

        // Subscribe to room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;

        // Subscribe to the points scored event
        StaticEventHandler.OnPointsScored += StaticEventHandler_OnPointsScored;

        // Subscribe to the score multiplier event
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;

        // Subscribe to the player destroyed event
        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }

    private void OnDisable()
    {
        // Unsubscribe to room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        // Unsubscribe to room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;

        // Unsubscribe to the points scored event
        StaticEventHandler.OnPointsScored -= StaticEventHandler_OnPointsScored;

        // Unsubscribe to the score multiplier event
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;

        // Unsubscribe to the player destroyed event
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
    }

    // Handle room changed event
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }

    // Handle the room enemies defeated event
    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        RoomEnemiesDefeated();
    }


    // Handle the points scored event
    private void StaticEventHandler_OnPointsScored(PointsScoredArgs pointsScoredArgs)
    {
        // Increase the score
        gameScore += pointsScoredArgs.points * scoreMultiplier;

        // Call score changed event
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {
        if (multiplierArgs.multiplier)
        {
            scoreMultiplier++;
        }
        else
        {
            scoreMultiplier--;
        }

        // Clamp the multiplier
        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30);

        // Call the score changed event to update
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    // Handle the player destroyed event
    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.gameLost;
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameState.gameStarted;
        previousGameState = GameState.gameStarted;

        // Initialise game score
        gameScore = 0;

        // Initialise multiplier
        scoreMultiplier = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleGameState();

        // Testing the dungeon generations
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = GameState.gameStarted;
        }
    }

    // Handle the game state
    private void HandleGameState()
    {
        // Handle game state
        switch (gameState)
        {
            case GameState.gameStarted:
                // Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;

                // Trigger room enemies defeated
                RoomEnemiesDefeated();
                break;
            case GameState.levelCompleted:
                // Display level completed text here
                StartCoroutine(LevelCompleted());
                break;
            case GameState.gameWon:
                if (previousGameState != GameState.gameWon)
                {
                    StartCoroutine(GameWon());
                }
                break;
            case GameState.gameLost:
                if (previousGameState != GameState.gameLost)
                {
                    StopAllCoroutines();
                    StartCoroutine(GameLost());
                }
                break;
            case GameState.restartGame:
                RestartGame();
                break;
        }
    }

    // Set the current room the player is in
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }

    // Room enemies defeated - test if all of the dungeon rooms have been cleared
    private void RoomEnemiesDefeated()
    {
        // Initialise the dungeon as being cleared, then test each room
        bool isDungeonClearOfRegularEnemies = true;
        bossRoom = null;

        // Loop through all of the dungeon rooms to see if they are cleared
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            if (keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

            // Check if the other rooms are cleared
            if (!keyValuePair.Value.isClearedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        // Check if the dungeon has been completed
        if ((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isClearedOfEnemies))
        {
            // Are there any more dungeon levels?
            if (currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.levelCompleted;
            }
            else
            {
                gameState = GameState.gameWon;
            }
        }
        // Else if the dungeon level cleared apart from the boss room
        else if (isDungeonClearOfRegularEnemies)
        {
            gameState = GameState.bossStage;

            StartCoroutine(BossStage());
        }
    }


    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Could not build dungeon from specified rooms and node graphs.");
        }

        // Call static event that room has changed
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        // Set player roughly mid-room
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // Get the nearest spawn point in the room closest to the player
        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }

    // Enter the boss stage
    private IEnumerator BossStage()
    {
        // Activate the boss room
        bossRoom.gameObject.SetActive(true);

        // Unlock the boss room
        bossRoom.UnlockDoors(0f);

        // Wait
        yield return new WaitForSeconds(2f);

        Debug.Log("You have reached the Boss Stage.");
    }

    // Show level as being completed
    private IEnumerator LevelCompleted()
    {
        // Play the next level
        gameState = GameState.playingLevel;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        Debug.Log("Level Completed.");

        // When the player presses the return key we can proceed to next level
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        currentDungeonLevelListIndex++;

        PlayDungeonLevel(currentDungeonLevelListIndex);
    }

    // Game won
    private IEnumerator GameWon()
    {
        previousGameState = GameState.gameWon;

        Debug.Log("Game WON!");

        yield return new WaitForSeconds(10f);

        // Set game state to restart game
        gameState = GameState.restartGame;
    }

    // Game lost
    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        Debug.Log("Game LOST!");

        yield return new WaitForSeconds(10f);

        // Set game state to restart game
        gameState = GameState.restartGame;
    }

    // Restart the game
    private void RestartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    // Get the player
    public Player GetPlayer()
    {
        return player;
    }

    // Get the player minimap icon
    public Sprite GetPlayerMiniMapIcon()
    {
        return playerDetails.playerMiniMapIcon;
    }

    // Get the current room that the player is in
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    // Get current dungeon level
    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }
#endif
    #endregion Validation
}
