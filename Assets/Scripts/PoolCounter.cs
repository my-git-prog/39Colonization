using TMPro;
using UnityEngine;

public class PoolCounter<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private Pool<T> _pool;
    [SerializeField] private TextMeshProUGUI _activeText;
    [SerializeField] private TextMeshProUGUI _nonActiveText;

    private void OnEnable()
    {
        _pool.CountsChanged += ChangeText;
    }

    private void OnDisable()
    {
        _pool.CountsChanged -= ChangeText;
    }

    private void ChangeText()
    {
        _activeText.text = _pool.ActiveCount.ToString();
        _nonActiveText.text = _pool.NonActiveCount.ToString();
    }
}
