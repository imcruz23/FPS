using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickBack : MonoBehaviour
{
    public Rigidbody _rb;
    public float _kickBackForce;
    public void KickBackPlayer()
    {
        _rb.AddForce(-Camera.main.transform.forward * _kickBackForce, ForceMode.Impulse);
    }
}
