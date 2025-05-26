using System;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private T _itemPrefab;

    private Pool<T> _pool = new Pool<T>();

    public T GetItem()
    {
        if (_pool.TryGetItem(out T item))
        {
            return item;
        }

        return Instantiate(_itemPrefab);
    }

    public void ReleaseItem(T item)
    {
        _pool.ReleaseItem(item);
    }
}
