using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Engine _engine;
    [SerializeField] private float _constantForcePower;
    [SerializeField] private CowCatcher _catcher;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private ConstantForce _constantForce;
    private Transform _transform;
    private float _inputY;

    private void Awake()
    {
        _transform = transform;
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _constantForce = GetComponent<ConstantForce>();
        _catcher.SetInput(_playerInput);

        _engine.Initialize(_rigidbody);
    }

    private void FixedUpdate()
    {
        _constantForce.force = -Vector3.right * _playerInput.Controls.x * _constantForcePower + Physics.gravity * _rigidbody.mass;
    }

    private void Update()
    {
        var isVerticalAsixActive = !Mathf.Approximately(_playerInput.Controls.y, 0);

        if (isVerticalAsixActive)
        {
            _engine.SetAltitude(_engine.GetCurrentAltitude());
            _engine.SetOverridedControls(_playerInput.Controls.y);
        }

        _engine.IsOverrided = isVerticalAsixActive;
    }
}
