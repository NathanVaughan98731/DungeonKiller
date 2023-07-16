using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;
    private Player player;
    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immunityTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.2f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamageable = true;
    [HideInInspector] public Enemy enemy;

    private void Awake()
    {
        // Load components
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        // Trigger the health event to update UI
        CallHealthEvent(0);

        // Attempt to load enemy / player components
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        // Get the player and enemy hit immunity details
        if (player != null)
        {
            if (player.playerDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = player.playerDetails.hitImmunityTime;
                spriteRenderer = player.spriteRenderer;
            }
        }
        else if (enemy != null)
        {
            if (enemy.enemyDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = enemy.enemyDetails.hitImmunityTime;
                spriteRenderer = enemy.spriteRendererArray[0];
            }
        }

        // Enable the health bar 
        if (enemy != null && enemy.enemyDetails.isHealthBarDisplayed == true && healthBar != null)
        {
            healthBar.EnableHealthBar();
        }
        else if (healthBar != null)
        {
            healthBar.DisableHealthBar();
        }

    }

    // Use this method to apply damage
    public void TakeDamage(int damageAmount)
    {
        bool isRolling = false;

        if (player != null)
        {
            isRolling = player.playerControl.isPlayerRolling;
        }

        if (isDamageable && !isRolling)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            PostHitImmunity();
        }

        // Set the health bar when damaged
        if (healthBar != null)
        {
            healthBar.SetHealthBarValue((float)currentHealth / (float)startingHealth);
        }
    }

    // Indicate a hit and give the post hit immunity
    private void PostHitImmunity()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }

        // If there is a post hit immunity
        if (isImmuneAfterHit)
        {
            if (immunityCoroutine != null)
            {
                StopCoroutine(immunityCoroutine);
            }

            // Flash red and give preiod of immunity for agent
            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRenderer));
        }
    }

    // Coroutine for the post hit immunity
    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(immunityTime / spriteFlashInterval / 2f);
        isDamageable = false;

        while (iterations > 0)
        {
            spriteRenderer.color = Color.red;
            yield return WaitForSecondsSpriteFlashInterval;

            spriteRenderer.color = Color.white;
            yield return WaitForSecondsSpriteFlashInterval;

            iterations--;

            yield return null;
        }

        isDamageable = true;
    }

    private void CallHealthEvent(int damageAmount)
    {
        // Trigger the health event
        healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }

    // Set starting health
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    // Get the starting health
    public int GetStartingHealth()
    {
        return startingHealth;
    }
}
