using System.Collections;
using UnityEngine;

public class SpawnerResources : Spawner<Resource>
{
    [SerializeField] private float _periodOfSpawn;
    [SerializeField] private bool _isSpawning;
    [SerializeField] private float _minimumPositionX;
    [SerializeField] private float _maximumPositionX;
    [SerializeField] private float _minimumPositionZ;
    [SerializeField] private float _maximumPositionZ;
    [SerializeField] private float _positionY;

    private void Start()
    {
        StartCoroutine(StartResourcesSpawning());
    }

    private IEnumerator StartResourcesSpawning()
    {
        var wait = new WaitForSeconds(_periodOfSpawn);

        while (_isSpawning)
        {
            GetItem().Initialize(GetRandomSpawnPosition());

            yield return wait;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(_minimumPositionX, _maximumPositionX), _positionY, 
            Random.Range(_minimumPositionZ, _maximumPositionZ));
    }
}
