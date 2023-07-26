using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerShoot : MonoBehaviour
{
    //public PlayerGunSelector _playerGunSelector;
    public static Action OnShoot;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0))
        {
            //_playerGunSelector._activeGun.Tick(Mouse.current.leftButton.isPressed);
            //_playerGunSelector._activeGun.Shoot();
            OnShoot?.Invoke();
        }
    }
}
