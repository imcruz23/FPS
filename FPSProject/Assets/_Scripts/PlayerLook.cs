using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public GameObject _cam;

    [Header("Sensitivity")]
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    // Multiplicador para girar camara
    float _multiplier = 0.01f;

    float _xRot;
    float _yRot;

    // Variables para guardar los datos del raton moviendose
    float _mouseX;
    float _mouseY;

    void Start()
    {
        //_cam = Camera.main;
        SetCursor();
    }



    // Update is called once per frame
    void Update()
    {
        CheckRotation();
    }
    private void LateUpdate()
    {
        HandleRotation();
    }

    private void CheckRotation()
    {
        _mouseX = Input.GetAxisRaw("Mouse X");
        _mouseY = Input.GetAxisRaw("Mouse Y");

        _yRot += (_mouseX * _sensX * _multiplier);
        _xRot -= (_mouseY * _sensY * _multiplier);

        _xRot = Mathf.Clamp(_xRot, -90f, 90f);
    }

    private void HandleRotation()
    {
        _cam.transform.localRotation = Quaternion.Euler(_xRot, 0 , 0);
        transform.rotation = Quaternion.Euler(0, _yRot , 0);
    }

    private void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public float GetHorizontalCameraInput() { return _mouseX; }
    public float GetVerticalCameraInput() { return _mouseY; }
}
