using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rayLength = 50f;

    private PlayerInput _playerInput;
    private Vector3 _lastTargetPosition;
    private Tweener _tween;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _lastTargetPosition = transform.position;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Camera.Move.performed += OnMove;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Camera.Move.performed -= OnMove;
    }

    private void Start()
    {
        _tween = transform.DOMove(_lastTargetPosition, _duration).SetAutoKill(false);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputAxes = context.action.ReadValue<Vector2>();
        Vector3 newTargetPosition = _lastTargetPosition + new Vector3(inputAxes.x, 0f, inputAxes.y) * _speed;

        if (Physics.Raycast(newTargetPosition, transform.forward, out RaycastHit hit, _rayLength) == false)
        {
            return;
        }

        _tween.ChangeEndValue(newTargetPosition, true).Restart();
        _lastTargetPosition = newTargetPosition;
    }
}
