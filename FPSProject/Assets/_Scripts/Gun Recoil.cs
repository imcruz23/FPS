using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] Vector3 _rotationalRecoil;
    [SerializeField] Vector3 _positionalRecoil;

    [Header("Speed Settings")]
    [SerializeField] float _rotationalRecoilSpeed;
    [SerializeField] float _positionalRecoilSpeed;
    [SerializeField] float _positionalReturnSpeed;
    [SerializeField] float _rotationalReturnSpeed;


    //Kickback
    [SerializeField] Vector3 _recoilKickBack;

    [Header("Reference Points")]
    // Transform del arma
    [SerializeField] private Transform _recoilPosition;
    [SerializeField] private Transform _recoilRotation;

    private Vector3 _gunPosition;

    Vector3 _rot;

    [Header("Player Gun Movement")]
    public float _kickBackForce;
    private void Start()
    {
        _gunPosition = transform.localPosition;
    }
    private void LateUpdate()
    {
        
        _rotationalRecoil = Vector3.Lerp(_rotationalRecoil, Vector3.zero, _rotationalReturnSpeed * Time.deltaTime);
        _positionalRecoil = Vector3.Lerp(_positionalRecoil, Vector3.zero, _positionalRecoilSpeed * Time.deltaTime);

        // Aplicando a los transforms
        _recoilPosition.localPosition = Vector3.Slerp(_recoilPosition.localPosition, _positionalRecoil, _positionalRecoilSpeed * Time.deltaTime);
        _rot = Vector3.Slerp(_rot, _rotationalRecoil, _rotationalRecoilSpeed * Time.deltaTime);
        _recoilPosition.localRotation = Quaternion.Euler(_rot);

        //GoBack();
        
    }

    public void RecoilWeapon()
    {
        _rotationalRecoil += new Vector3(-_recoilRotation.rotation.x, Random.Range(-_recoilRotation.rotation.y, _recoilRotation.rotation.y), Random.Range(-_recoilRotation.rotation.z, _recoilRotation.rotation.z));
        _positionalRecoil += new Vector3(Random.Range(-_recoilKickBack.x, _recoilKickBack.x), _recoilKickBack.y, -_recoilKickBack.z);
    }

    void GoBack()
    {
        _positionalRecoil = Vector3.Lerp(_positionalRecoil, _gunPosition, _positionalRecoilSpeed * Time.deltaTime);
        transform.localPosition = _positionalRecoil;
    }
}
