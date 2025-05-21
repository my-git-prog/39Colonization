using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, ISpawnable
{
    private List<T> _nonActiveItems;

    public int NonActiveCount => _nonActiveItems.Count;

    public Pool()
    {
        _nonActiveItems = new List<T>();
    }

    public void ReleaseItem(T item)
    {
        _nonActiveItems.Add(item);
        item.gameObject.SetActive(false);
    }

    public bool TryGetItem(out T item)
    {
        if ( _nonActiveItems.Count > 0)
        {
            item = _nonActiveItems[0];
            _nonActiveItems.Remove(item);

            return true;
        }

        item = null;

        return false;
    }
}
