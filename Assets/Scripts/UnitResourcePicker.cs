using UnityEngine;

public class UnitResourcePicker : MonoBehaviour
{
    private Resource _resource;
    private bool _hasResource = false;

    public bool HasResource => _hasResource;

    private void Update()
    {
        if(_hasResource)
        {
            _resource.transform.position = transform.position;
            _resource.transform.rotation = transform.rotation;
        }
    }

    public void SetResource(Resource resource)
    {
        _hasResource = true;
        _resource = resource;
    }

    public Resource GetResource()
    {
        _hasResource = false;
        return _resource;
    }
}
