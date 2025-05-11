using System;
using UnityEngine;

public class UnitResourceFinder : MonoBehaviour
{
    public event Action<Resource> Finded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            Finded?.Invoke(resource);
        }
    }
}
