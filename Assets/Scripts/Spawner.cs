using System;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private T _itemPrefab;

    private Pool<T> _pool = new Pool<T>();

    public event Action CountChanged;

    public int NonActiveCount => _pool.NonActiveCount;

    private void Start()
    {
        CountChanged?.Invoke();
    }

    public T GetItem()
    {
        if (_pool.TryGetItem(out T item))
        {
            CountChanged?.Invoke();

            return item;
        }

        return Instantiate(_itemPrefab);
    }

    public void ReleaseItem(T item)
    {
        CountChanged?.Invoke();
        _pool.ReleaseItem(item);
    }
}
