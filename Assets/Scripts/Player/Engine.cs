using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [HideInInspector]
    public bool IsOverrided = false;

    [Header("Spherecast")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _spherecastRadius;
    [SerializeField] private float _maxDistance;

    [Header("Lift")]
    [SerializeField] private float _maxForce;
    [SerializeField] private float _damping;

    private Transform _transform;
    private Rigidbody _targetBody;
    private float _springSpeed;
    private float _oldDistance;
    private float _altitude;
    private float _distance;
    private float _inputY;

    public float GetCurrentAltitude()
    {
        if (Physics.SphereCast(_transform.position, _spherecastRadius, _transform.forward, out RaycastHit hitInfo, _maxDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.distance;
        }

        return _maxDistance;
    }

    public void SetAltitude(float altitude)
    {
        _altitude = Mathf.Clamp(altitude, _spherecastRadius, _maxDistance);
    }

    public void Initialize(Rigidbody targetBody)
    {
        _transform = transform;
        _targetBody = targetBody;
    }

    private void FixedUpdate()
    {
        if (_targetBody == null)
            return;

        var forward = _transform.forward;

        if (IsOverrided)
            ForceUpDown(forward);
        else
            Lift(forward);

        Damping();
    }

    private void ForceUpDown(Vector3 forward)
    {
        float forceFactor = (_inputY > 0) ? 1 : 0;

        _targetBody.AddForce(-forward * Mathf.Max(forceFactor * _maxForce - _springSpeed * _maxForce * _damping, 0f), ForceMode.Force);
    }

    public void SetOverridedControls(float inputY)
    {
        _inputY = inputY;
    }

    private void Lift(Vector3 forward)
    {
        if (Physics.SphereCast(_transform.position, _spherecastRadius, forward, out RaycastHit hitInfo, _maxDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            _distance = hitInfo.distance;

            Damping();

            var minForceHeight = _altitude + 1f;
            var maxForceHeight = _altitude - 1f;

            var forceFactor = Mathf.Clamp(_distance, maxForceHeight, minForceHeight).Remap(maxForceHeight, minForceHeight, 1, 0);
            _targetBody.AddForce(-forward * Mathf.Max(forceFactor * _maxForce - _springSpeed * _maxForce * _damping, 0f), ForceMode.Force);
        }
    }

    private void Damping()
    {
        _springSpeed = (_distance - _oldDistance) * Time.fixedDeltaTime;
        _springSpeed = Mathf.Max(_springSpeed, 0);
        _oldDistance = _distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        var startPoint = transform.position;
        var endPoint = transform.position + transform.forward * _maxDistance;

        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
        Gizmos.DrawLine(startPoint, endPoint);
        Gizmos.DrawSphere(endPoint, _spherecastRadius);
    }
}
