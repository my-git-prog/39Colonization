using System.Collections;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private HomeResourcesScanner _resourcesScanner;
    [SerializeField] private HomeUnitsOrganizer _unitsOrganizer;
    [SerializeField] private float _periodScanningResources;
    [SerializeField] private int _costOfUnit;

    private int _resources = 0;
    private bool _isAlive = true;

    private void Start()
    {
        StartCoroutine(PeriodicalSendUnitsToResources());
    }

    private void OnEnable()
    {
        _unitsOrganizer.ResourceNotBringed += _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed += _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed += OnResourceReceived;
    }

    private void OnDisable()
    {
        _unitsOrganizer.ResourceNotBringed -= _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed -= _resourcesScanner.RemoveFromFinded;
        _unitsOrganizer.ResourceBringed -= OnResourceReceived;
    }

    private IEnumerator PeriodicalSendUnitsToResources()
    {
        var wait = new WaitForSeconds(_periodScanningResources);

        while(_isAlive)
        {
            if (_unitsOrganizer.FreeUnitsCount > 0)
                _unitsOrganizer.SendUnitsToResources(_resourcesScanner.GetResources(_unitsOrganizer.FreeUnitsCount));

            yield return wait;
        }
    }

    private void OnResourceReceived(Resource resource)
    {
        _resources++;
        _spawnerResources.ReleaseItem(resource);

        if (_resources >= _costOfUnit)
        {
            _resources -= _costOfUnit;
            _unitsOrganizer.CreateUnit();
        }
    }
}
