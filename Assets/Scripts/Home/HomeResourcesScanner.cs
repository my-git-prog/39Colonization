using System;
using System.Collections.Generic;
using UnityEngine;

public class HomeResourcesScanner : MonoBehaviour
{
    [SerializeField] private float _scanningRadius;
    [SerializeField] private float _maximumResourcesPositionY;
    [SerializeField] private int _findingCollidersCount;

    private List<Resource> _allTimeFindedResources;
    private List<Resource> _findedResources;
    private Collider[] _findedColliders;

    private void Awake()
    {
        _allTimeFindedResources = new List<Resource>();
        _findedResources = new List<Resource>();
        _findedColliders = new Collider[_findingCollidersCount];
    }

    public void RemoveFromFinded(Resource resource)
    {
        if(_allTimeFindedResources.Contains(resource))
            _allTimeFindedResources.Remove(resource);
    }

    public List<Resource> GetResources(int count)
    {
        Array.Clear(_findedColliders, 0, _findedColliders.Length);
        _findedResources.Clear();

        Physics.OverlapSphereNonAlloc(transform.position, _scanningRadius, _findedColliders);

        foreach (Collider collider in _findedColliders)
        {
            if (collider == null)
                return _findedResources;

            if (collider.gameObject.transform.position.y < _maximumResourcesPositionY)
            {
                if (collider.gameObject.TryGetComponent(out Resource resource))
                {
                    if (_allTimeFindedResources.Contains(resource) == false)
                    {
                        if (resource.transform.parent == null)
                        {
                            _findedResources.Add(resource);
                            _allTimeFindedResources.Add(resource);

                            if (--count == 0)
                                return _findedResources;
                        }
                    }
                }
            }
        }

        return _findedResources;
    }
}
