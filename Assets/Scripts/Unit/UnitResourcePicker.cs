using UnityEngine;

public class UnitResourcePicker : MonoBehaviour
{
    private Resource _resource;

    public void TakeResource(Resource resource)
    {
        _resource = resource;
        resource.transform.SetParent(transform);
        resource.ResetLocalParametres();
    }

    public Resource GiveResource()
    {
        if (_resource == null)
            return null;

        Resource resource = _resource;
        _resource.transform.SetParent(null);
        _resource = null;
        return resource;
    }
}
