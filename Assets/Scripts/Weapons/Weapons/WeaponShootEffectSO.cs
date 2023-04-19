using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponShootEffect_", menuName = "Scriptable Objects/Weapons/Weapon Shoot Effect")]
public class WeaponShootEffectSO : ScriptableObject
{
    #region Header WEAPON SHOOT EFFECT DETAILS
    [Space(10)]
    [Header("WEAPON SHOOT EFFECT DETAILS")]
    #endregion Header WEAPON SHOOT EFFECT DETAILS

    #region Tooltip
    [Tooltip("The colour gradient for the shoot effect. This gradient shows the colour of particles during their lifetime.")]
    #endregion Tooltip
    public Gradient colourGradient;

    #region Tooltip
    [Tooltip("The length of time the particle system is emitting particles.")]
    #endregion Tooltip
    public float duration = 0.5f;

    #region Tooltip
    [Tooltip("The start particle size for the particle effect")]
    #endregion Tooltip
    public float startParticleSize = 0.25f;

    #region Tooltip
    [Tooltip("The start particle speed for the particle effect")]
    #endregion Tooltip
    public float startParticleSpeed = 3f;

    #region Tooltip
    [Tooltip("The particle lifetime for the particle effect")]
    #endregion Tooltip
    public float startLifetime = 0.5f;

    #region Tooltip
    [Tooltip("The maximum number of particles to be emitted")]
    #endregion
    public int maxParticleNumber = 100;

    #region Tooltip
    [Tooltip("The number of particles emitted per second. If zero it will just be the burst number")]
    #endregion
    public int emissionRate = 100;

    #region Tooltip
    [Tooltip("How many particles should be emitted in the particle effect burst")]
    #endregion Tooltip
    public int burstParticleNumber = 20;

    #region Tooltip
    [Tooltip("The gravity on the particles")]
    #endregion Tooltip
    public float effectGravity = -0.01f;

    #region Tooltip
    [Tooltip("The sprite for the particle effect")]
    #endregion Tooltip
    public Sprite sprite;

    #region Tooltip
    [Tooltip("The minimum velocity for the particle over its lifetime.")]
    #endregion Tooltip
    public Vector3 velocityOverLifetimeMin;

    #region Tooltip
    [Tooltip("The maximum velocity for the particle over its lifetime.")]
    #endregion Tooltip
    public Vector3 velocityOverLifetimeMax;

    #region Tooltip
    [Tooltip("weaponShootEffectPrefab contains the particle system for the shoot effect")]
    #endregion
    public GameObject weaponShootEffectPrefab;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(duration), duration, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSize), startParticleSize, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startLifetime), startLifetime, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(emissionRate), emissionRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootEffectPrefab), weaponShootEffectPrefab);
    }
#endif
    #endregion Validation
}
