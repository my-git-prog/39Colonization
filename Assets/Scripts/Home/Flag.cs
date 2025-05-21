using System;
using UnityEngine;

public class Flag : MonoBehaviour, IFindable
{
    public event Action Moved;

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        Moved?.Invoke();
    }
}
