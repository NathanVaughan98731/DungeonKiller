using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponShootEffect : MonoBehaviour
{
    private ParticleSystem shootEffectParticleSystem;

    private void Awake()
    {
        // Load components
        shootEffectParticleSystem = GetComponent<ParticleSystem>();
    }

    // Set the shoot effect from the passed in WeaponShootEffectSO and aimAngle
    public void SetShootEffect(WeaponShootEffectSO shootEffect, float aimAngle)
    {
        // Set shoot effect colour gradient
        SetShootEffectColourGradient(shootEffect.colourGradient);

        // Set shoot effect particle system starting values
        SetShootEffectParticleStartingValues(shootEffect.duration, shootEffect.startParticleSize, shootEffect.startParticleSpeed, shootEffect.startLifetime,
            shootEffect.effectGravity, shootEffect.maxParticleNumber);

        // Set shoot effect particle system burst particle number
        SetShootEffectEmission(shootEffect.emissionRate, shootEffect.burstParticleNumber);

        // Set emitter rotation
        SetEmitterRotation(aimAngle);

        // Set shoot effect lifetime min and max velocities
        SetShootEffectVelocityOverLifeTime(shootEffect.velocityOverLifetimeMin, shootEffect.velocityOverLifetimeMax);
    }

    // Set the shoot effect colour gradient
    private void SetShootEffectColourGradient(Gradient gradient)
    {
        ParticleSystem.ColorOverLifetimeModule colourOverLifetimeModule = shootEffectParticleSystem.colorOverLifetime;
        colourOverLifetimeModule.color = gradient;
    }

    // Set shoot effect particle system starting values
    private void SetShootEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
    {
        ParticleSystem.MainModule mainModule = shootEffectParticleSystem.main;

        // Set particle system duration
        mainModule.duration = duration;

        // Set particle start size
        mainModule.startSize = startParticleSize;

        // Set particle start speed
        mainModule.startSpeed = startParticleSpeed;

        // Set particle start lifetime
        mainModule.startLifetime = startLifetime;

        // Set particle starting gravity
        mainModule.gravityModifier = effectGravity;

        // Set max particles
        mainModule.maxParticles = maxParticles;
    }

    // Set shoot effect particle system burst particle number
    private void SetShootEffectEmission(int emissionRate, float burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = shootEffectParticleSystem.emission;

        /// Set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        // Set particle emission rate
        emissionModule.rateOverTime = emissionRate;
    }

    // Set shoot effect particle system sprite
    private void SetShootEffectParticleSprite(Sprite sprite)
    {
        // Set particle burst number
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = shootEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);
    }

    // Set the rotation of the emitter to match the aim angle
    private void SetEmitterRotation(float aimAngle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
    }

    // Set the shoot effect velocity over lifetime
    private void SetShootEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = shootEffectParticleSystem.velocityOverLifetime;

        // Define min max X velocity
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = minVelocity.x;
        minMaxCurveX.constantMax = maxVelocity.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        // Define min max Y velocity
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y;
        minMaxCurveY.constantMax = maxVelocity.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        // Define min max Z velocity
        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z;
        minMaxCurveZ.constantMax = maxVelocity.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;
    }
}
