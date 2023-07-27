using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplingHook : MonoBehaviour
{
    //Position of camera and player
    public Transform _camPosition;
    public Transform _player;

    public LayerMask _grappleableMask;
   
    public float _sphereRadius;
    public float _maxDistance;

    // Inside variables
    private SpringJoint _joint;
    private LineRenderer _lineRenderer;
    private Vector3 _grapplePoint;


    private void Awake()
    {
        TryGetComponent(out _lineRenderer);
        _camPosition = Camera.main.transform;
    }

    private void Start()
    {
        PlayerShoot.OnHookActivate += StartGrapple;
        PlayerShoot.OnHookDeactivate += StopGrapple;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
            StartGrapple();
        else if (Input.GetKeyUp(KeyCode.F))
            StopGrapple();
        */
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        if (Physics.SphereCast(_camPosition.position, _sphereRadius, _camPosition.forward, out RaycastHit hit, _maxDistance, _grappleableMask))
        {
            _grapplePoint = hit.point;
            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _grapplePoint;

            float distanceFromPoint = Vector3.Distance(_player.position, _grapplePoint);

            _joint.maxDistance = distanceFromPoint * 0.8f;
            _joint.minDistance = distanceFromPoint * 0.3f;

            // Adjust values
            _joint.spring = 7f;
            _joint.damper = 10f;
            _joint.massScale = 4.5f;

            _lineRenderer.positionCount = 2;
        }
    }

    private void DrawRope()
    {
        if (!_joint) return;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _grapplePoint);
    }

    private void StopGrapple()
    {
        _lineRenderer.positionCount = 0;
        Destroy(_joint);
    }
}
