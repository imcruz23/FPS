using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerShoot : MonoBehaviour
{
    //public PlayerGunSelector _playerGunSelector;
    public static Action OnShoot;
    public static Action OnHookActivate;
    public static Action OnHookDeactivate;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0))
        {
            //_playerGunSelector._activeGun.Tick(Mouse.current.leftButton.isPressed);
            //_playerGunSelector._activeGun.Shoot();
            OnShoot?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
            OnHookActivate?.Invoke();
        else if (Input.GetKeyUp(KeyCode.F))
            OnHookDeactivate?.Invoke();

        if(Input.GetMouseButton(1))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.5f, Time.fixedDeltaTime);
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, Time.fixedDeltaTime);
        }
    }
}
