using System;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _sqrDestinateDistance;

    private Vector3 _targetPosition;

    public Action Reached;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if ((transform.position - _targetPosition).sqrMagnitude < _sqrDestinateDistance)
        {
            Reached?.Invoke();
        }
    }

    public void ChangeTarget(Vector3 position)
    {
        _targetPosition = position;
        transform.LookAt(_targetPosition);
    }
}
