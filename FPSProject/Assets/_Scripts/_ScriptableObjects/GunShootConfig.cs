using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapon/Shoot Config", order = 3)]
public class GunShootConfig : ScriptableObject
{
    public Vector3 _spread;
    public int _bulletsPerTap;


    public Vector3 GenerateSpread()
    {
        return new Vector3(
                    Random.Range(-_spread.x, _spread.x),
                    Random.Range(-_spread.y, _spread.y),
                    Random.Range(-_spread.z, _spread.z)
                );
    }
}
