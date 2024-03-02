using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class QuaternionRotation : MonoBehaviour
{
    [SerializeField] private float _angle;
    [SerializeField] private Transform _axis;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    [ProPlayButton]
    public void Rotate()
    {
        Quaternion quaternion = Quaternion.AngleAxis(_angle,_axis.forward);

        _transform.rotation = quaternion * _transform.rotation;
    }

    private void Update()
    {
        Quaternion quaternion = Quaternion.AngleAxis(_angle * Time.deltaTime, _axis.forward);

        quaternion = Quaternion.Slerp(quaternion, Quaternion.Euler(Vector3.zero), 0.5f);

        _transform.rotation = quaternion * _transform.rotation;
    }

    private void OnDrawGizmos()
    {
        if (_axis == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, _axis.forward * 10f);
        Gizmos.DrawRay(transform.position, -_axis.forward * 10f);
    }
}
