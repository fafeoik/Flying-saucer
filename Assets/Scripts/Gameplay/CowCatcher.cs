using System;
using UnityEngine;

public class CowCatcher : MonoBehaviour
{
    [SerializeField] private float _catchDistance;
    [SerializeField] private float _catchRadius;
    [SerializeField] private GameObject _effect;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _catchTime;

    private bool _isCatchActionActive = false;
    private Transform _transform;
    private float _catchTimer = -1;
    private Transform _catchedCow;
    private Vector3 _startCowPosition;
    private Vector3 _startCowScale;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (_catchTimer > 0)
        {
            _catchTimer -= Time.deltaTime / _catchTime;

            if (_catchTimer <= 0)
            {
                if (_catchedCow != null)
                {
                    Destroy(_catchedCow.gameObject);
                    _catchedCow = null;
                    OnCatchReleased();
                }
            }
        }

        if (_catchedCow != null)
            UpdateCowTransform();
    }

    private void FixedUpdate()
    {
        if (!_isCatchActionActive)
            return;

        if (_catchedCow != null)
            return;

        var colliders = Physics.OverlapSphere(_transform.position + _transform.forward * _catchDistance, _catchRadius, _layerMask, QueryTriggerInteraction.Ignore);

        foreach (var collider in colliders)
        {
            var cow = collider.GetComponentInParent<Cow>();

            if (cow != null)
            {
                cow.Catched();
                _catchedCow = cow.transform;
                _catchedCow.SetParent(_transform);
                _startCowPosition = _catchedCow.localPosition;
                _startCowScale = _catchedCow.localScale;

                _catchTimer = 1f;

                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + transform.forward * _catchDistance, _catchRadius);
    }

    public void SetInput(PlayerInput input)
    {
        input.CatchPressed += OnCatchPressed;
        input.CatchReleased += OnCatchReleased;
    }

    private void OnCatchReleased()
    {
        if (_catchedCow != null)
            return;

        SetCatch(false);
    }

    private void SetCatch(bool value)
    {
        _effect.SetActive(value);
        _isCatchActionActive = value;
    }

    private void OnCatchPressed()
    {
        SetCatch(true);
    }

    private void UpdateCowTransform()
    {
        float t = Mathf.SmoothStep(0, 1, _catchTimer);

        _catchedCow.transform.localPosition = Vector3.Lerp(Vector3.zero, _startCowPosition, t);
        _catchedCow.transform.localScale = Vector3.Lerp(Vector3.zero, _startCowScale, t);
    }
}
