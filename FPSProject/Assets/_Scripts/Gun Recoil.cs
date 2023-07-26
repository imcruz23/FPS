using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] Vector3 _rotationalRecoil;
    [SerializeField] Vector3 _positionalRecoil;
    [SerializeField] float _rotationalRecoilSpeed;
    [SerializeField] float _positionalRecoilSpeed;

    //Kickback
    [SerializeField] Vector3 _recoilKickBack;

    // Transform del arma
    [SerializeField] private Transform _recoilPosition;
    [SerializeField] private Transform _recoilRotation;

    private Vector3 _gunPosition;

    Vector3 _rot;
    private void LateUpdate()
    {
        
        _rotationalRecoil = Vector3.Lerp(_rotationalRecoil, Vector3.zero, _rotationalRecoilSpeed * Time.deltaTime);
        _positionalRecoil = Vector3.Lerp(_positionalRecoil, Vector3.zero, _positionalRecoilSpeed * Time.deltaTime);

        // Aplicando a los transforms
        _recoilPosition.localPosition = Vector3.Slerp(transform.localPosition, _positionalRecoil, _positionalRecoilSpeed * Time.deltaTime);
        _rot = Vector3.Slerp(_rot, _rotationalRecoil, _rotationalRecoilSpeed * Time.deltaTime);
        _recoilPosition.localRotation = Quaternion.Euler(_rot);
        
    }

    public void RecoilWeapon()
    {
        _rotationalRecoil += new Vector3(-_recoilRotation.rotation.x, Random.Range(-_recoilRotation.rotation.y, _recoilRotation.rotation.y), Random.Range(-_recoilRotation.rotation.z, _recoilRotation.rotation.z));
        _positionalRecoil += new Vector3(Random.Range(-_recoilKickBack.x, _recoilKickBack.x), Random.Range(-_recoilKickBack.y, _recoilKickBack.y), Random.Range(-_recoilKickBack.z, _recoilKickBack.z));
    }


}
