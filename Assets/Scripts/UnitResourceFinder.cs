using System;
using UnityEngine;

public class UnitResourceFinder : MonoBehaviour
{
    public Action<Resource> Finded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            if (resource.IsFree)
            {
                Finded?.Invoke(resource);
                resource.Get();
            }
        }
    }
}
