using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header PLAYER BASE DETAILS
    [Space(10)]
    [Header("PLAYER BASE DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("Player Character Name")]
    #endregion 
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("Prefab GameObject for the Player")]
    #endregion 
    public GameObject playerPrefab;

    #region Tooltip
    [Tooltip("Player Runtime Animator Controller")]
    #endregion 
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header HEALTH
    [Space(10)]
    [Header("HEALTH")]
    #endregion
    #region Tooltip
    [Tooltip("Player Starting Health Amount")]
    #endregion 
    public int playerHealthAmount;

    #region Header WEAPON
    [Space(10)]
    [Header("WEAPON")]
    #endregion
    #region Tooltip
    [Tooltip("Player initial starting weapon")]
    #endregion
    public WeaponDetailsSO startingWeapon;
    #region Tooltip
    [Tooltip("Populate with the list of starting weapons")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList;



    #region Header OTHER
    [Space(10)]
    [Header("OTHER")]
    #endregion
    #region Tooltip
    [Tooltip("Player Icon Sprite to be used in the Minimap")]
    #endregion 
    public Sprite playerMiniMapIcon;

    #region Tooltip
    [Tooltip("Player Hand Sprite")]
    #endregion 
    public Sprite playerHandSprite;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMiniMapIcon), playerMiniMapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);
    }
#endif
    #endregion
}
