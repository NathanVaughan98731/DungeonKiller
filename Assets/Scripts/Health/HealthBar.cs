using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    #region Header GAMEOBJECT REFERENCES
    [Space(10)]
    [Header("GAMEOBJECT REFERENCES")]
    #endregion Header GAMEOBJECT REFERENCES

    #region Tooltip
    [Tooltip("Populate with the child bar gameobejct.")]
    #endregion Tooltip

    [SerializeField] private GameObject healthBar;

    // Enable the health bar
    public void EnableHealthBar()
    {
        gameObject.SetActive(true);
    }

    // Disable the health bar
    public void DisableHealthBar()
    {
        gameObject.SetActive(false);
    }

    // Set the health bar value between 0 and 1
    public void SetHealthBarValue(float healthPercent)
    {
        healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}
