using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Engine _engine;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        _engine.Initialize(_rigidbody);
    }
}
