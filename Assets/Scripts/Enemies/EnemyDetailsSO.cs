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
    }
#endif
    #endregion
}
