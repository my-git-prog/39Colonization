using System;
using UnityEngine;

public class Unit : MonoBehaviour, ISpawnable
{
    [SerializeField] private UnitResourceFinder _resourceFinder;
    [SerializeField] private UnitHomeFinder _homeFinder;
    [SerializeField] private UnitResourcePicker _resourcePicker;
    [SerializeField] private UnitMover _mover;

    private Vector3 _targetPosition;
    private Vector3 _homePosition;

    public Action<Resource> ResourceBringed;
    public Action<Unit> UnitReturned;

    private void OnEnable()
    {
        _resourceFinder.Finded += OnResourceFinded;
        _homeFinder.Finded += OnHomeFinded;
        _mover.Reached += OnMoveTargetReached;
    }

    private void OnDisable()
    {
        _resourceFinder.Finded -= OnResourceFinded;
        _homeFinder.Finded -= OnHomeFinded;
    }

    public void Initialize(Home home)
    {
        _homePosition = home.transform.position;
        _homeFinder.Initialize(home);
        transform.position = home.transform.position;
        gameObject.SetActive(false);
    }

    private void OnResourceFinded(Resource resource)
    {
        _resourcePicker.gameObject.SetActive(true);
        _resourcePicker.SetResource(resource);
        ReturnBase();
    }

    private void OnMoveTargetReached()
    {
        ReturnBase();
    }

    private void OnHomeFinded()
    {
        if(_resourcePicker.HasResource)
        {
            ResourceBringed?.Invoke(_resourcePicker.GetResource());
            _resourcePicker.gameObject.SetActive(false);
        }

        UnitReturned?.Invoke(this);
    }

    private void ReturnBase()
    {
        _mover.ChangeTarget(_homePosition);
        _resourceFinder.gameObject.SetActive(false);
        _homeFinder.gameObject.SetActive(true);
    }

    public void FindResource(Vector3 position)
    {
        _mover.ChangeTarget(position);
        _resourceFinder.gameObject.SetActive(true);
        _homeFinder.gameObject.SetActive(false);
    }
}
