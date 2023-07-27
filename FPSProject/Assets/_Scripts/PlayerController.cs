using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 _movement; // Vector de movimiento
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _slopeMoveDirection;
    private Vector3 _mouseLook;
    private Rigidbody _rb;

    private float _playerHeight = 3.6f;

    [Header("Speed variables")]
    public float _moveSpeed = 6f;
    public float _acceleration;

    // Controlar que estas en el suelo
    [Header("Ground Layer")]
    public LayerMask _groundLayerMask;
    private bool _isGrounded;
    private float _groundDistance = 0.1f;

    //[SerializeField] Transform _groundHit;

    private RaycastHit _slopeHit; // Controlar cuestas

    [Header("Jump Settings")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] float _jumpForce;
    float _airAcceleration = 0.5f;

    [Header("Drag")]
    float _groundDrag = 6f;
    float _airDrag = 2f;

    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _rb);
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = CheckGrounded();
        //Debug.Log(_isGrounded);
        CheckInput();
        ControlDrag();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private bool CheckGrounded()
    {
   
        return Physics.CheckSphere(transform.position - new Vector3(0, _playerHeight/2, 0), _groundDistance, _groundLayerMask);
        //return Physics.Raycast(_groundHit.position, Vector3.down * 0.01f, _groundLayerMask);
    }

    private bool isOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight / 2 + 0.5f))
        {
            return _slopeHit.normal != Vector3.up; // Estamos en un slope porque la normal no apunta hacia arriba
        }
        return false;
    }
    private void CheckInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        // Comprobar el input de salto
        if (Input.GetKeyDown(_jumpKey) && IsGrounded)
            Jump();
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        //_rb.AddForce(transform.up * Mathf.Sqrt(_jumpForce * -2f * Physics.gravity.y), ForceMode.Impulse);
    }

    private void HandleMovement()
    {
        // Aplicamos movimiento de forma separada a cada componente
        _movement = transform.forward * _verticalInput + transform.right * _horizontalInput;
        // Clampeamos a 1 el movimiento
        _movement = Vector3.ClampMagnitude(_movement, 1f);
        if (isOnSlope())
            _slopeMoveDirection = Vector3.ProjectOnPlane(_movement, _slopeHit.normal);
    }

    private void MovePlayer()
    {
        if (IsGrounded) //Movimiento normal
            //_rb.velocity =  _movement * _currentSpeed;
            _rb.AddForce(_movement.normalized * _moveSpeed * _acceleration, ForceMode.Acceleration);

        else if (IsGrounded && isOnSlope()) //Movimiento si hay cuestas
            //_rb.velocity = _slopeMoveDirection * _currentSpeed;
            _rb.AddForce(_slopeMoveDirection.normalized * _moveSpeed * _acceleration, ForceMode.Acceleration);

        else if (!IsGrounded) //Movimiento si el jugador esta cayendo
            _rb.AddForce(_movement.normalized * _moveSpeed * (_acceleration * _airAcceleration), ForceMode.Acceleration);
    }

    private void ControlDrag()
    {
        if (IsGrounded)
            _rb.drag = _groundDrag;
        else
            _rb.drag = _airDrag;
    }

    public float GetHorizontalInput() {return _horizontalInput; }
    public float GetVerticalInput() { return _verticalInput; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, _playerHeight / 2, 0), _groundDistance);
    }
}
