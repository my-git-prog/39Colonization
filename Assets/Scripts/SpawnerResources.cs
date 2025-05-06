using System.Collections;
using UnityEngine;

public class SpawnerResources : Spawner<Resource>
{
    [SerializeField] private float _periodOfSpawn = 1f;
    [SerializeField] private bool _isSpawning = true;
    [SerializeField] private float _minimumPositionX = -10f;
    [SerializeField] private float _maximumPositionX = 10f;
    [SerializeField] private float _minimumPositionZ = -10f;
    [SerializeField] private float _maximumPositionZ = 10f;
    [SerializeField] private float _positionY = 1f;

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
