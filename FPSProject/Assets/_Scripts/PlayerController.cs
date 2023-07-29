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
    private float _groundDistance = 0.2f;

    //[SerializeField] Transform _groundHit;

    private RaycastHit _slopeHit; // Controlar cuestas

    [Header("Jump Settings")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] float _jumpForce;
    float _airAcceleration = 0.5f;

    [Header("Drag")]
    float _groundDrag = 6f;
    float _airDrag = 2f;

    [Header("Slope Settings")]
    public float _maxSteepAngle = 60f;

    // Input buffer
    Queue<KeyCode> _buffer;

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
        _buffer = new Queue<KeyCode>();
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

    /// <summary>
    /// Check if we are in a slope
    /// </summary>
    /// <returns>True if we are on a slope, false if we are not</returns>
    private bool isOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSteepAngle && angle != 0; // Estamos en un slope porque la normal no apunta hacia arriba

        }
        return false;
    }

    private bool IsWalkableSlope(RaycastHit p_hit)
    {
        if (Vector3.Angle(p_hit.normal, Vector3.down) < _maxSteepAngle)// walkable
        {
            return true;
        }
        // Not walkable
        return false;
    }

    private void CheckInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // Check Input Jump Key
        if (Input.GetKeyDown(_jumpKey))
        {
            // Save input in the buffer
            _buffer.Enqueue(_jumpKey);
           Invoke(nameof(DequeueAction), 0.5f); // Dequeue action if its made > 0.5s before making contact with the ground

            if(IsGrounded) // Check if grounded
            {
                if (_buffer.Count > 0) // If there is something in the buffer
                {
                    if (_buffer.Peek() == KeyCode.Space) // Check if the first action is jumping
                    {
                        Debug.Log("Buffer");
                        Jump(); // Do jumping
                        _buffer.Dequeue(); // Dequeuing action
                    }
                }
            }  
        }     
    }

    // Dequeuing an action if its made too early
    private void DequeueAction()
    {
        if(_buffer.Count > 0)
            _buffer.Dequeue();
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        //_rb.AddForce(transform.up * Mathf.Sqrt(_jumpForce * -2f * Physics.gravity.y), ForceMode.Impulse);
    }

    private void HandleMovement()
    {
        _rb.AddForce(Vector3.down * 10f * Time.deltaTime); //Extra gravity for adding more ground velocity

        // Aplicamos movimiento de forma separada a cada componente
        _movement = transform.forward * _verticalInput + transform.right * _horizontalInput;

        // Clampeamos a 1 el movimiento
        _movement = Vector3.ClampMagnitude(_movement, 1f);

        if (isOnSlope())
            _slopeMoveDirection = Vector3.ProjectOnPlane(_movement, _slopeHit.normal).normalized;
    }

    private void MovePlayer()
    {
        if (isOnSlope()) // Slope movement
        {
            _rb.AddForce(_slopeMoveDirection * _moveSpeed * _acceleration, ForceMode.Force);

            if (_rb.velocity.y > 0) // Adding extra gravity if we are bumping on a slope
                _rb.AddForce(Vector3.down * 70f, ForceMode.Force);
        }
        else if(IsGrounded) // Ground movement
                _rb.AddForce(_movement.normalized * _moveSpeed * _acceleration, ForceMode.Force);

        else if (!IsGrounded) // Air movement
            _rb.AddForce(_movement.normalized * _moveSpeed * (_acceleration * _airAcceleration), ForceMode.Force);

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
