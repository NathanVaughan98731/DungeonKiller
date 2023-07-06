using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;

    [HideInInspector] public bool isDamageable = true;

    private void Awake()
    {
        // Load components
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        // Trigger the health event to update UI
        CallHealthEvent(0);
    }

    // Use this method to apply damage
    public void TakeDamage(int damageAmount)
    {
        if (isDamageable)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);
        }
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
