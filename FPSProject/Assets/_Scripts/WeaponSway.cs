using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerLook _playerLook;
    [SerializeField] private Rigidbody _rb;

    // Almacenar los valores de forma independiente para persistencia

    private Vector3 _playerMovement;
    private Vector3 _mouseMovement;

    [Header("Settings")]
    public bool _sway;
    public bool _swayRotation;
    public bool _bobOffset;
    public bool _bobSway;

    [Header("Sway")]
    public float _step = 0.01f; // Factor de multiplicacion del raton por cada frame
    public float _maxStepDistance = 0.06f; // maxima distancia desde el origen local
    Vector3 _swayPos;

    [Header("SwayRotation")]
    public float _rotationStep = 4f;
    public float _maxRotationStep = 5f;
    Vector3 _swayEulerRot;

    [Header("Smoothness")]
    public float _smooth = 10f;
    public float _smoothRot = 12f;

    [Header("Bobbing")]
    public float _speedCurve;
    float _curveSin { get => Mathf.Sin(_speedCurve); }
    float _curveCos { get => Mathf.Cos(_speedCurve); }

    public Vector3 _travelLimit = Vector3.one * 0.025f;
    public Vector3 _bobLimit = Vector3.one * 0.01f;

    Vector3 _bobPosition;

    [Header("Bob Rotation")]
    public Vector3 _multiplier;
    Vector3 _bobEulerRotation;

    // Update is called once per frame
    void Update()
    {
        (_playerMovement, _mouseMovement) = GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();
        CompositePositionRotation();
    }

    private void BobOffset()
    {
        _speedCurve += Time.deltaTime * (_playerController.IsGrounded ? _rb.velocity.magnitude : 1f) + 0.01f;

        if (!_bobOffset) { _bobPosition = Vector3.zero; return; }

        _bobPosition.x = (_curveCos * _bobLimit.x * (_playerController.IsGrounded ? 1 : 0)) - (_playerMovement.x * _travelLimit.x); // bob - input
        _bobPosition.y = (_curveSin * _bobLimit.y) - (_rb.velocity.y * _travelLimit.y); // bob - offset Y
        _bobPosition.z = -(_playerMovement.z * _travelLimit.z); // - input
    }

    private void BobRotation()
    {
        if (!_bobSway) { _bobEulerRotation = Vector3.zero; return; }

        _bobEulerRotation.x = (
            _playerMovement != Vector3.zero ?
            _multiplier.x * (Mathf.Sin(2 * _speedCurve)) :
            _multiplier.x * (Mathf.Sin(2 * _speedCurve) / 2)); // pitch
        _bobEulerRotation.y = (_playerMovement != Vector3.zero ?
            _multiplier.y * _curveCos : 0); // yaw
        _bobEulerRotation.z = (_playerMovement != Vector3.zero ?
            _multiplier.z * _curveCos * _playerMovement.x : 0); // roll
    }
    private void Sway()
    {
        if (!_sway) { _swayPos = Vector3.zero; return; }

        Vector3 invertLook = _mouseMovement * -_step;
        invertLook.x = Mathf.Clamp(invertLook.x, -_maxStepDistance, _maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -_maxStepDistance, _maxStepDistance);
        _swayPos = invertLook;
    }

    private void SwayRotation()
    {
        if (!_swayRotation) { _swayPos = Vector3.zero; return; }

        Vector2 invertLook = _mouseMovement * -_rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, _maxRotationStep, _maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, _maxRotationStep, _maxRotationStep);

        _swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _swayPos + _bobPosition,
            Time.deltaTime * _smooth
            );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.Euler(_swayEulerRot) * Quaternion.Euler(_bobEulerRotation),
            Time.deltaTime * _smoothRot
            );
    }

    private (Vector3, Vector3) GetInput()
    {
        Vector3 playerMov = Vector3.zero;
        playerMov.x = _playerController.GetHorizontalInput();
        playerMov.y = _playerController.GetVerticalInput();
        playerMov.Normalize();

        Vector3 playerLook = Vector3.zero;
        playerLook.x = _playerLook.GetHorizontalCameraInput();
        playerLook.y = _playerLook.GetVerticalCameraInput();

        return (playerMov, playerLook);
    }

}
