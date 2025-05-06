using TMPro;
using UnityEngine;

public class SpawnersCounter<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private Spawner<T> _spawner;
    [SerializeField] private TextMeshProUGUI _activeText;
    [SerializeField] private TextMeshProUGUI _nonActiveText;

    private void OnEnable()
    {
        _spawner.CountsChanged += ChangeText;
    }

    private void OnDisable()
    {
        _spawner.CountsChanged -= ChangeText;
    }

    private void ChangeText()
    {
        _activeText.text = _spawner.ActiveCount.ToString();
        _nonActiveText.text = _spawner.NonActiveCount.ToString();
    }
}
