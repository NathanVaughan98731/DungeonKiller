using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void InititaliseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overriseAmmoMovement = false);

    GameObject GetGameObject();
}
