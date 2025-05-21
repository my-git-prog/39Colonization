using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUnitsOrganizer : Spawner<Unit>
{
    [SerializeField] private int _minimumUnitsCount;
    [SerializeField] private float _unitsOutcomingDistance;
    [SerializeField] private float _periodSendingUnits;
    [SerializeField] private bool _isSendingUnits;

    private List<Unit> _freeUnits;
    private List<Unit> _workingUnits;
    private Dictionary<Unit, Resource> _busyUnitsResources;
    private Queue<Unit> _waitingSandingUnits;

    public event Action<Resource> ResourceBringed;
    public event Action<Resource> ResourceNotBringed;
    public event Action<Unit> FlagFinded;

    public int FreeUnitsCount => _freeUnits.Count;
    public int UnitsCount => _freeUnits.Count + _workingUnits.Count;
    public int MinimumUnitsCount => _minimumUnitsCount;

    private void Awake()
    {
        _freeUnits = new List<Unit>();
        _workingUnits = new List<Unit>();
        _busyUnitsResources = new Dictionary<Unit, Resource>();
        _waitingSandingUnits = new Queue<Unit>();
    }

    private void Start()
    {
        StartCoroutine(SendUnitsToWork());
    }

    public void Initialize(int startingUnitsCount)
    {
        for (int i = 0; i < startingUnitsCount; i++)
        {
            CreateUnit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (_busyUnitsResources.ContainsKey(unit))
            {
                if (unit.GiveResource() == null)
                    ResourceNotBringed?.Invoke(_busyUnitsResources[unit]);
                else
                    ResourceBringed?.Invoke(_busyUnitsResources[unit]);

                _workingUnits.Remove(unit);
                _freeUnits.Add(unit);
                _busyUnitsResources.Remove(unit);
                unit.gameObject.SetActive(false);
            }
        }
    }

    public void CreateUnit()
    {
        Unit unit = GetItem();
        unit.Initialize(transform.position);
        _freeUnits.Add(unit);
    }

    public void SendUnitsToResources(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            if(FreeUnitsCount > 0)
            {
                Unit unit = _freeUnits[0];

                _busyUnitsResources.Add(unit, resource);
                _freeUnits.Remove(unit);
                _workingUnits.Add(unit);
                unit.transform.position = transform.position + 
                    (resource.transform.position - transform.position).normalized * _unitsOutcomingDistance;
                unit.FindResource(resource);
                _waitingSandingUnits.Enqueue(unit);
            }
        }
    }

    public void SendUnitToBuildHome(Flag flag)
    {
        if (FreeUnitsCount > 0)
        {
            Unit unit = _freeUnits[0];
            _freeUnits.Remove(unit);
            unit.transform.position = transform.position +
                (flag.transform.position - transform.position).normalized * _unitsOutcomingDistance;
            unit.BuildHome(flag);
            unit.FlagFinded += OnFlagFinded;
            _waitingSandingUnits.Enqueue(unit);
        }
    }

    private IEnumerator SendUnitsToWork()
    {
        var wait = new WaitForSeconds(_periodSendingUnits);

        while(_isSendingUnits)
        {
            if (_waitingSandingUnits.Count > 0)
                _waitingSandingUnits.Dequeue().gameObject.SetActive(true);

            yield return wait;
        }
    }

    public void AddUnit(Unit unit)
    {
        _freeUnits.Add(unit);
        unit.Initialize(transform.position);
    }

    private void OnFlagFinded(Unit unit)
    {
        unit.FlagFinded -= OnFlagFinded;
        FlagFinded?.Invoke(unit);
    }
}
