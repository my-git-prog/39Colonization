using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private T _itemPrefab;

    private List<T> _activeItems;
    private List<T> _nonActiveItems;

    public Action CountsChanged;

    public int ActiveCount => _activeItems.Count;
    public int NonActiveCount => _nonActiveItems.Count;

    private void Awake()
    {
        _activeItems = new List<T>();
        _nonActiveItems = new List<T>();
    }

    public T GetItem()
    {
        T item;

        if(_nonActiveItems.Count > 0)
        {
            item = _nonActiveItems[0];
            _nonActiveItems.Remove(item);
        }
        else
        {
            item = Instantiate(_itemPrefab);
        }

        _activeItems.Add(item);
        CountsChanged?.Invoke();

        return item;
    }

    public void ReleaseItem(T item)
    {
        if(_activeItems.Contains(item))
        {
            _activeItems.Remove(item);
            _nonActiveItems.Add(item);
        }

        item.gameObject.SetActive(false);
        CountsChanged?.Invoke();
    }
}
