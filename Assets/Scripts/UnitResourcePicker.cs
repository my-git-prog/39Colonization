using UnityEngine;

public class UnitResourcePicker : MonoBehaviour
{
    private Resource _resource;

    public void SetResource(Resource resource)
    {
        _resource = resource;
        resource.transform.SetParent(transform);
        resource.ResetLocalParametres();
    }

    public Resource GetResource()
    {
        if (_resource == null)
            return null;

        Resource resource = _resource;
        _resource.transform.SetParent(null);
        _resource = null;
        return resource;
    }
}
