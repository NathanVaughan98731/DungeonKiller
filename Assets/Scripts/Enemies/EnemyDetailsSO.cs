using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/EnemyDetails")]
public class EnemyDetailsSO : ScriptableObject
{
    #region Header BASE ENEMY DETAILS
    [Space(10)]
    [Header("BASE ENEMY DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The name of the enemy.")]
    #endregion
    public string enemyName;

    #region Tooltip
    [Tooltip("The prefab for the enemy.")]
    #endregion
    public GameObject enemyPrefab;

    #region Tooltip
    [Tooltip("Distance to the player before enemy starts chasing.")]
    #endregion
    public float chaseDistance = 50f;

    #region Header ENEMY MATERIAL
    [Space(10)]
    [Header("Enemy Material")]
    #endregion
    #region Tooltip
    [Tooltip("This is the standard lit shader material for the enemy (used after the enemy materialises).")]
    #endregion
    public Material enemyStandardMaterial;

    #region Header ENEMY MATERIALISE SETTINGS
    [Space(10)]
    [Header("Enemy Materialise Settings")]
    #endregion

    #region Tooltip
    [Tooltip("The time in seconds that it takes the enemy to materialise.")]
    #endregion
    public float enemyMaterialiseTime;
    #region Tooltip
    [Tooltip("The shader to be used when the enemy materialises.")]
    #endregion
    public Shader enemyMaterialiseShader;
    [ColorUsage(true, true)]
    #region Tooltip
    [Tooltip("The colour to use when the enemy materialises. This is an HDR colour so intensity can be set to cause glow/bloom")]
    #endregion
    public Color enemyMaterialiseColor;

    #region Header ENEMY WEAPON SETTINGS
    [Space(10)]
    [Header("Enemy Weapon Settings")]
    #endregion
    #region Tooltip
    [Tooltip("The weapon for the enemy.")]
    #endregion
    public WeaponDetailsSO enemyWeapon;

    #region Tooltip
    [Tooltip("The minimum time delay interval in seconds between bursts of enemy shooting. This value should be > 0. A random value will be selected between the minimum value and the maximum value.")]
    #endregion
    public float firingIntervalMin = 0.1f;

    #region Tooltip
    [Tooltip("The maximum time delay interval in seconds between bursts of enemy shooting. This value should be > 0. A random value will be selected between the minimum value and the maximum value.")]
    #endregion
    public float firingIntervalMax = 1f;

    #region Tooltip
    [Tooltip("The minimum firing duration that the enemy shoots for during a firing burst.")]
    #endregion
    public float firingDurationMin = 1f;

    #region Tooltip
    [Tooltip("The maximum firing duration that the enemy shoots for during a firing burst.")]
    #endregion
    public float firingDurationMax = 2f;

    #region Tooltip
    [Tooltip("Select this if line of sight is required of the player before the enemy fires. If line of sight is not selected the enemy will fire regardless of the obstacles whenever the player is in range.")]
    #endregion
    public bool firingLineOfSightRequired;

    #region Header ENEMY HEALTH
    [Space(10)]
    [Header("Enemy Health")]
    #endregion

    #region Tooltip
    [Tooltip("The health of the enemy for each corresponding level.")]
    #endregion
    public EnemyHealthDetails[] enemyHealthDetailsArray;
    #region Tooltip
    [Tooltip("Set this to true if you want enemy to be immune immediately after being hit.")]
    #endregion
    public bool isImmuneAfterHit = false;
    #region Tooltip
    [Tooltip("Immunity time in seconds after being hit.")]
    #endregion
    public float hitImmunityTime;

    #region Tooltip
    [Tooltip("Select to display a health bar for the enemy or not.")]
    #endregion
    public bool isHealthBarDisplayed = false;

    #region Validation
#if UNITY_EDITOR
    // Validate the scriptable object details
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyStandardMaterial), enemyStandardMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(enemyMaterialiseTime), enemyMaterialiseTime, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyMaterialiseShader), enemyMaterialiseShader);

        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin, nameof(firingIntervalMax), firingDurationMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax), firingDurationMax, false);

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyHealthDetailsArray), enemyHealthDetailsArray);

        if (isImmuneAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }
    }
#endif
    #endregion
}
