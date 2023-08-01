using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed.")]
    #endregion Tooltip
    [SerializeField] private MovementDetailsSO movementDetails;
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>();
    private Vector3 playerReferencePosition;
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public float moveSpeed;
    private bool chasePlayer = false;
    [HideInInspector] public int updateFrameNumber = 1;
    private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();

    private void Awake()
    {
        // Load components
        enemy = GetComponent<Enemy>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        // Create waitForFixed update to be used in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        // Reset the player reference position
        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
    }

    private void Update()
    {
        MoveEnemy();
    }

    // Use A* pathfinding to build a path to the player and then move the enemy to each grid location on the path.
    private void MoveEnemy()
    {
        // Movement cooldown timer
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        // Check distance to player to see if enemy should start chasing
        if (!chasePlayer && Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.enemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }

        // If the enemy is not close enough to chase player then return
        if (!chasePlayer)
        {
            return;
        }

        // Only process A* path building on certain frames to spread the load between enemies
        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != updateFrameNumber)
        {
            return;
        }

        // If the movement cooldown timer is reached or the player has moved more than the required distance
        // Rebuild the enemy path and move the enemy
        if (currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePosition, GameManager.Instance.GetPlayer().GetPlayerPosition()) > Settings.playerMoveDistanceToRebuildPath))
        {
            // Reset the path and rebuild cooldown timer
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;

            // Reset player reference position
            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            // Move the enemy using A* pathfinding
            CreatePath();

            // If a path has been found, move the enemy
            if (movementSteps != null)
            {
                if (moveEnemyRoutine != null)
                {
                    // Trigger the idle event
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }

                // Move enemy along the path using a coroutine
                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }
        }
    }

    // Coroutine to move the enemy to the next location on the path
    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while (movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            // While not very close continue to move, when close move onto the next step
            while (Vector3.Distance(nextPosition, transform.position) > 0.2f)
            {
                // Trigger the movement event
                enemy.movementToPositionEvent.CallMovementToPositionEvent(nextPosition, transform.position, moveSpeed, (nextPosition - transform.position).normalized);

                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }

        // End of the path steps - trigger the enemy idle event
        enemy.idleEvent.CallIdleEvent();
    }

    // Use the A* static class to create a path for the enemy
    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();
        Grid grid = currentRoom.instantiatedRoom.grid;

        // Get player position on the grid
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom);

        // Get enemy position on the grid
        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);


        // Build a path for the enemy to move on
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        if (movementSteps != null)
        {
            movementSteps.Pop();
        }
        else
        {
            // Trigger the idle event since there is no path
            enemy.idleEvent.CallIdleEvent();
        }
    }

    // Set the frame number that the enemy path will be recalculated on, this is to avoid any performance spikes for building a path
    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }

    // Get the nearest position to the player that is not on an obstacle
    private Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom)
    {
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
        Vector3Int playerCellPosition = currentRoom.instantiatedRoom.grid.WorldToCell(playerPosition);
        Vector2Int adjustedPlayerCellPosition = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x, playerCellPosition.y - currentRoom.templateLowerBounds.y);

        int obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y], currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y]);

        // If the player is not on a cell square that is marked as an obstacle then return the position
        if (obstacle != 0)
        {
            return playerCellPosition;
        }
        else
        {
            // Empty the surrounding position list
            surroundingPositionList.Clear();

            // Populate the surrounding position list which will hold the 8 possible vector locations around (0,0) grid
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0)
                    {
                        continue;
                    }

                    surroundingPositionList.Add(new Vector2Int(i, j));
                }
            }

            // Iterate through all of the positions
            for (int k = 0; k < 8; k++)
            {
                // Generate a random index
                int index = Random.Range(0, surroundingPositionList.Count);

                // Check if there is an obstacle in the currently selected surrounding position
                try
                {
                    obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x + surroundingPositionList[index].x, adjustedPlayerCellPosition.y + surroundingPositionList[index].y], currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPosition.x + surroundingPositionList[index].x, adjustedPlayerCellPosition.y + surroundingPositionList[index].y]);
                
                    if (obstacle != 0)
                    {
                        return new Vector3Int(playerCellPosition.x + surroundingPositionList[index].x, playerCellPosition.y + surroundingPositionList[index].y);
                    }
                }
                catch
                {

                }

                // Remove the surrounding position with the obstacle so that we can try again
                surroundingPositionList.RemoveAt(index);
            }

            // If there is no non-obstacle cells found surrounding the player
            return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion Validation


}
