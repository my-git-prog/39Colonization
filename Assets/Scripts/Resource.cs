using UnityEngine;

public class Resource : MonoBehaviour, ISpawnable
{
    private bool isFree = true;

    public bool IsFree => isFree;

    public void Get()
    {
        isFree = false;
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.identity;
        isFree = true;
        gameObject.SetActive(true);
    }
}