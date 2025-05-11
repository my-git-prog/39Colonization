using UnityEngine;

public class Unit : MonoBehaviour, ISpawnable
{
    [SerializeField] private UnitResourceFinder _resourceFinder;
    [SerializeField] private UnitResourcePicker _resourcePicker;
    [SerializeField] private UnitMover _mover;

    private Resource _targetResource;

    private void OnEnable()
    {
        _resourceFinder.Finded += OnResourceFinded;
        _mover.TargetReached += ReturnBase;
    }

    private void OnDisable()
    {
        _resourceFinder.Finded -= OnResourceFinded;
        _mover.TargetReached -= ReturnBase;
    }

    public void Initialize(Vector3 position)
    {
        _mover.Initialize(position);
        transform.position = position;
        gameObject.SetActive(false);
    }

    private void OnResourceFinded(Resource resource)
    {
        if (resource == _targetResource)
        {
            _resourcePicker.SetResource(resource);
            ReturnBase();
        }
    }

    private void ReturnBase()
    {
        _mover.ReturnHome();
        _resourceFinder.gameObject.SetActive(false);
    }

    public void FindResource(Resource resource)
    {
        _targetResource = resource;
        _mover.ChangeTarget(resource.transform.position);
        _resourceFinder.gameObject.SetActive(true);
    }

    public Resource GetResource()
    {
        return _resourcePicker.GetResource();
    }
}
