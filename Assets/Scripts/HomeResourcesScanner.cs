using System.Collections.Generic;
using UnityEngine;

public class HomeResourcesScanner : MonoBehaviour
{
    [SerializeField] private float _scanningRadius;
    [SerializeField] private float _maximumResourcesPositionY;

    private List<Resource> _findedResources;

    private void Awake()
    {
        _findedResources = new List<Resource>();
    }

    public void RemoveFromFinded(Resource resource)
    {
        if(_findedResources.Contains(resource))
            _findedResources.Remove(resource);
    }

    public List<Resource> GetResources(int count)
    {
        List<Resource> resources = new List<Resource>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanningRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.transform.position.y < _maximumResourcesPositionY)
            {
                if (collider.gameObject.TryGetComponent(out Resource resource))
                {
                    if (_findedResources.Contains(resource) == false)
                    {
                        if (resource.transform.parent == null)
                        {
                            resources.Add(resource);
                            _findedResources.Add(resource);

                            if (--count == 0)
                                return resources;
                        }
                    }
                }
            }
        }

        return resources;
    }
}
