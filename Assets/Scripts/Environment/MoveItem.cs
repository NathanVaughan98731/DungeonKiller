using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveItem : MonoBehaviour
{
    #region SOUND EFFECT
    [Header("SOUND EFFECT")]
    #endregion SOUND EFFECT
    #region Tooltip
    [Tooltip("The sound effect when we move the item")]
    #endregion Tooltip
    [SerializeField] private SoundEffectSO moveSoundEffect;

    public BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    private InstantiatedRoom instantiatedRoom;
    private Vector3 previousPosition;

    private void Awake()
    {
        // Get component references
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        instantiatedRoom = GetComponentInParent<InstantiatedRoom>();

        // Add this item to the obstacles array
        instantiatedRoom.moveableItemsList.Add(this);
    }

    // Update the obstacle positions when something comes into contact
    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateObstacles();
    }

    // Update the obstacles position
    private void UpdateObstacles()
    {
        // Make sure the item stays within the room
        KeepItemToRoomBounds();

        // Update moveable items in obstacles array
        instantiatedRoom.UpdateMoveableObstacles();

        // Capture the new position after collision
        previousPosition = transform.position;

        // Play the sound
        if (Mathf.Abs(rigidbody2D.velocity.x) > 0.001f || Mathf.Abs(rigidbody2D.velocity.y) > 0.001f)
        {
            if (moveSoundEffect != null && Time.frameCount % 10 == 0)
            {
                SoundEffectManager.Instance.PlaySoundEffect(moveSoundEffect);
            }
        }
    }

    // Keep the item within the rooms boundaries
    private void KeepItemToRoomBounds()
    {
        Bounds itemBounds = boxCollider2D.bounds;
        Bounds roomBounds = instantiatedRoom.roomColliderBounds;

        // If the item is being pushed past the room bounds
        if (itemBounds.min.x <= roomBounds.min.x || itemBounds.max.x >= roomBounds.max.x || itemBounds.min.y <= roomBounds.min.y || itemBounds.max.y >= roomBounds.max.y)
        {
            transform.position = previousPosition;
        }
    }









}
