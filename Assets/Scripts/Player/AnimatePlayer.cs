using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        // Subscribe to idle event
        player.idleEvent.OnIdle += IdleEvent_OnIdle;

        // Subscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        // Unsubscribe to idle event
        player.idleEvent.OnIdle -= IdleEvent_OnIdle;

        // Unsubscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    // On idle event handler
    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    // On weapon aim event handler
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitialiseAimAnimationParameters();

        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    // Initialise aim animation parameters
    private void InitialiseAimAnimationParameters()
    {
        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimUpRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimDown, false);
    }

    // Set movement animation parameters
    private void SetIdleAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true);
    }

    // Set aim animation parameters
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        // Set aim direction
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;
            case AimDirection.UpLeft:
                player.animator.SetBool(Settings.aimUpLeft, true);
                break;
            case AimDirection.UpRight:
                player.animator.SetBool(Settings.aimUpRight, true);
                break;
            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;
            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;
            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;
        }
    }
}
