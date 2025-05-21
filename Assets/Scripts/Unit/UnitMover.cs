using System;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _destinateDistance;

    private Vector3 _targetPosition;
    private Vector3 _homePosition;
    private float _sqrDestinateDistance;

    public event Action TargetReached;

    private void Awake()
    {
        _sqrDestinateDistance = _destinateDistance * _destinateDistance;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if ((transform.position - _targetPosition).sqrMagnitude < _sqrDestinateDistance)
        {
            TargetReached?.Invoke();
        }
    }

    public void Initialize(Vector3 position)
    {
        _homePosition = position;
    }

    public void ChangeTarget(Vector3 position)
    {
        _targetPosition = position;
        transform.LookAt(_targetPosition);
    }

    public void ReturnHome()
    {
        ChangeTarget(_homePosition);
    }
}
