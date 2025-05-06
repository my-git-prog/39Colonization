using System;
using UnityEngine;

public class UnitHomeFinder : MonoBehaviour
{
    private Home _home;

    public Action Finded;

    public void Initialize(Home home)
    {
        _home = home;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Home home))
        {
            if (home == _home)
            {
                Finded?.Invoke();
            }
        }
    }
}
