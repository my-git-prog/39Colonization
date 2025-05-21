using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] private Home _homePrefab;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private Camera _camera;

    private List<Home> _homes = new List<Home>();

    private void Start()
    {
        BuildHome(transform.position, null, 3);
    }

    private void OnDestroy()
    {
        foreach (var home in _homes)
        {
            home.HomeBuilding -= OnHomeBuilding;
        }
    }

    private void BuildHome(Vector3 position, Unit firstUnit, int startUnitsCount = 0)
    {
        Home home = Instantiate(_homePrefab, position, Quaternion.identity);
        home.Initialize(_spawnerResources, _camera, startUnitsCount);
        home.HomeBuilding += OnHomeBuilding;
        _homes.Add(home);

        if (firstUnit != null)
            home.AddUnit(firstUnit);
    }

    private void OnHomeBuilding(Vector3 position, Unit firstUnit)
    {
        BuildHome(position, firstUnit);
    }
}
