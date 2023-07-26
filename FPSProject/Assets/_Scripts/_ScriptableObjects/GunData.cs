using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GunData", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    public new string _name;

    public float _damage;
    public float _maxDistance;
    public float _fireRate;
}
