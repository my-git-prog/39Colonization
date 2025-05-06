using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] private int _startingUnitsCount;
    [SerializeField] private SpawnerUnits _spawnerUnits;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private float _minimumScanningRadius;
    [SerializeField] private float _maximumScanningRadius;
    [SerializeField] private float _stepScanningRadius;
    [SerializeField] private float _periodScanningResources;
    [SerializeField] private float _maximumResourcesPositionY;
    [SerializeField] private float _unitsOutcomingDistance;
    [SerializeField] private float _periodSendingUnits;
    [SerializeField] private int _costOfUnit;

    private int _resources = 0;
    private List<Unit> _freeUnits;
    private List<Unit> _workingUnits;
    private Dictionary<Unit, Resource> _findedResources;
    private Queue<Unit> _waitingSandingUnits;
    private bool _isAlive = true;

    private void Awake()
    {
        _freeUnits = new List<Unit>();
        _workingUnits = new List<Unit>();
        _findedResources = new Dictionary<Unit, Resource>();
        _waitingSandingUnits = new Queue<Unit>();
    }

    private void Start()
    {
        for (int i = 0; i < _startingUnitsCount; i++)
        {
            CreateNewUnit();
        }

        StartCoroutine(PeriodicalScanForResources());
        StartCoroutine(PeriodicalSendUnitsToResources());
    }

    private void ScanForResources()
    {
        if (_freeUnits.Count == 0)
            return;
        
        for (float r = _minimumScanningRadius; r <= _maximumScanningRadius; r += _stepScanningRadius)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, r);

            foreach (Collider collider in colliders)
            {
                if(collider.gameObject.transform.position.y  < _maximumResourcesPositionY)
                {
                    if (collider.gameObject.TryGetComponent(out Resource resource))
                    {
                        if (_findedResources.ContainsValue(resource) == false)
                        {
                            if(resource.IsFree)
                            {
                                Unit unit = _freeUnits[0];

                                SendUnitToResource(unit, resource);

                                if (_freeUnits.Count == 0)
                                    return;
                            }
                        }
                    }
                }
            }
        }
    }

    private void SendUnitToResource(Unit unit, Resource resource)
    {
        _findedResources.Add(unit, resource);
        _freeUnits.Remove(unit);
        _workingUnits.Add(unit);
        unit.transform.position = transform.position + (resource.transform.position - transform.position).normalized * _unitsOutcomingDistance;
        unit.FindResource(resource.transform.position);
        _waitingSandingUnits.Enqueue(unit);
    }

    private IEnumerator PeriodicalScanForResources()
    {
        var wait = new WaitForSeconds(_periodScanningResources);
        
        while(_isAlive)
        {
            ScanForResources();

            yield return wait;
        }
    } 

    private IEnumerator PeriodicalSendUnitsToResources()
    {
        var wait = new WaitForSeconds(_periodSendingUnits);

        while(_isAlive)
        {
            if(_waitingSandingUnits.Count > 0)
            {
                Unit unit = _waitingSandingUnits.Dequeue();
                unit.gameObject.SetActive(true);
                unit.ResourceBringed += OnResourceBringed;
                unit.UnitReturned += OnUnitReturned;
            }

            yield return wait;
        }
    }

    private void OnUnitReturned(Unit unit)
    {
        _workingUnits.Remove(unit);
        _freeUnits.Add(unit);
        _findedResources.Remove(unit);
        unit.ResourceBringed -= OnResourceBringed;
        unit.UnitReturned -= OnUnitReturned;
        unit.gameObject.SetActive(false);
    }

    private void OnResourceBringed(Resource resource)
    {
        _resources++;
        _spawnerResources.ReleaseItem(resource);

        if (_resources >= _costOfUnit)
        {
            _resources -= _costOfUnit;
            CreateNewUnit();
        }
    }

    private void CreateNewUnit()
    {
        Unit unit = _spawnerUnits.GetItem();
        unit.Initialize(this);
        _freeUnits.Add(unit);
    }
}
