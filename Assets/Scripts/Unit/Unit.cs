using System;
using UnityEngine;

public class Unit : MonoBehaviour, ISpawnable
{
    [SerializeField] private UnitResourceFinder _resourceFinder;
    [SerializeField] private UnitResourcePicker _resourcePicker;
    [SerializeField] private UnitMover _mover;
    [SerializeField] private UnitFlagFinder _flagFinder;

    private Resource _targetResource;
    private Flag _flag;

    public event Action <Unit> FlagFinded;

    private void OnEnable()
    {
        _resourceFinder.Finded += OnResourceFinded;
        _mover.TargetReached += ReturnBase;
        _flagFinder.Finded += OnFlagFinded;
    }

    private void OnDisable()
    {
        _resourceFinder.Finded -= OnResourceFinded;
        _mover.TargetReached -= ReturnBase;
        _flagFinder.Finded -= OnFlagFinded;
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
            _resourcePicker.TakeResource(resource);
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

    public Resource GiveResource()
    {
        return _resourcePicker.GiveResource();
    }

    public void BuildHome(Flag flag)
    {
        _flag = flag;
        _flag.Moved += OnFlagMoved;
        _mover.ChangeTarget(_flag.transform.position);
        _flagFinder.gameObject.SetActive(true);
    }

    private void OnFlagMoved()
    {
        _mover.ChangeTarget(_flag.transform.position);
    }

    private void OnFlagFinded(Flag flag)
    {
        if(flag == _flag)
        {
            _flag.Moved -= OnFlagMoved;
            _flag.gameObject.SetActive(false);
            _flagFinder.gameObject.SetActive(false);
            gameObject.SetActive(false);
            FlagFinded?.Invoke(this);
        }
    }
}
