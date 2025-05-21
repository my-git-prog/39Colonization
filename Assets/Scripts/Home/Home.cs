using System;
using System.Collections;
using UnityEngine;

public class Home : MonoBehaviour, ISelectable
{
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private HomeResourcesScanner _resourcesScanner;
    [SerializeField] private HomeUnitsOrganizer _unitsOrganizer;
    [SerializeField] private HomeFlagInstaller _flagInstaller;
    [SerializeField] private float _periodSendingUnitsToWork;
    [SerializeField] private int _costOfUnit;
    [SerializeField] private int _costOfHome;

    private int _resources = 0;
    private bool _isAlive = true;
    private bool _isBuildingNewHome = false;

    public event Action<Vector3, Unit> HomeBuilding;

    private void Start()
    {
        StartCoroutine(PeriodicalSendUnitsToWork());
    }

    private void OnEnable()
    {
        _unitsOrganizer.ResourceNotBringed += _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed += _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed += OnResourceReceived;
        _flagInstaller.Installed += OnFlagInstalled;
    }

    private void OnDisable()
    {
        _unitsOrganizer.ResourceNotBringed -= _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed -= _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed -= OnResourceReceived;
        _flagInstaller.Installed -= OnFlagInstalled;
    }

    private IEnumerator PeriodicalSendUnitsToWork()
    {
        var wait = new WaitForSeconds(_periodSendingUnitsToWork);

        while(_isAlive)
        {
            if (_unitsOrganizer.FreeUnitsCount > 0)
            {
                if(_isBuildingNewHome && _resources >= _costOfHome)
                {
                    _resources -= _costOfHome;
                    _unitsOrganizer.SendUnitToBuildHome(_flagInstaller.InstalledFlag);
                    _unitsOrganizer.FlagFinded += OnFlagFinded;
                    _isBuildingNewHome = false;
                }
                else
                {
                    _unitsOrganizer.SendUnitsToResources(_resourcesScanner.GetResources(_unitsOrganizer.FreeUnitsCount));
                }
            }

            yield return wait;
        }
    }

    private void OnResourceReceived(Resource resource)
    {
        _resources++;
        _spawnerResources.ReleaseItem(resource);

        if (_isBuildingNewHome && _unitsOrganizer.UnitsCount > _unitsOrganizer.MinimumUnitsCount)
        {
            return;
        }

        else if (_resources >= _costOfUnit)
        {
            _resources -= _costOfUnit;
            _unitsOrganizer.CreateUnit();
        }
    }

    public void Select()
    {
        _flagInstaller.gameObject.SetActive(true);
        _flagInstaller.ChoseFlagPlace();
    }

    private void OnFlagInstalled()
    {
        _isBuildingNewHome = true;
    }

    public void Initialize(SpawnerResources spawnerResources, Camera camera, int startingUnitsCount = 0)
    {
        _spawnerResources = spawnerResources;
        _flagInstaller.SetCamera(camera);
        _unitsOrganizer.Initialize(startingUnitsCount);
    }

    public void AddUnit(Unit unit)
    {
        _unitsOrganizer.AddUnit(unit);
    }

    private void OnFlagFinded(Unit unit)
    {
        _unitsOrganizer.FlagFinded -= OnFlagFinded;
        HomeBuilding?.Invoke(_flagInstaller.InstalledFlag.transform.position, unit);
    }
}
