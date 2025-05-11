using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Resource : MonoBehaviour, ISpawnable
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        transform.localRotation = Quaternion.identity;
        _rigidbody.isKinematic = false;
       _rigidbody.useGravity = true;
        gameObject.SetActive(true);
    }

    public void ResetLocalParametres()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }
}