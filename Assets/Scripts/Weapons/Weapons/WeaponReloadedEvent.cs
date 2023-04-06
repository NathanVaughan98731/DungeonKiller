using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class WeaponReloadedEvent : MonoBehaviour
{
    public event Action<WeaponReloadedEvent, WeaponReloadedEventArgs> OnWeaponReload;

    public void CallWeaponReloadedEvent(Weapon weapon)
    {
        OnWeaponReload?.Invoke(this, new WeaponReloadedEventArgs() { weapon = weapon });
    }
}

public class WeaponReloadedEventArgs : EventArgs
{
    public Weapon weapon;
}
