using System;
using UnityEngine;

public class UnitItemFinder<T> : MonoBehaviour where T : IFindable
{
    public event Action<T> Finded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T item))
        {
            Finded?.Invoke(item);
        }
    }
}
