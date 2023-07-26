using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    Vector3 _currentRotation;
    Vector3 _targetRotation;

    // Recoil del arma
    [SerializeField] float _recoilX;
    [SerializeField] float _recoilY;
    [SerializeField] float _recoilZ;

    // Ajustes
    [SerializeField] float _snapiness;
    [SerializeField] float _returnSpeed;

    private void LateUpdate()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snapiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void RecoilFire()
    {
        _targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }
}
