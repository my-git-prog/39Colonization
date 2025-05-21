using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Camera))]
public class SelectObjectListener : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 100f;
    private Camera _camera;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Mouse.LeftButtonClick.performed += TrySelectObject;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Mouse.LeftButtonClick.performed -= TrySelectObject;
    }

    private void TrySelectObject(InputAction.CallbackContext context)
    {
        Vector2 mousePos = _playerInput.Mouse.Position.ReadValue<Vector2>();
        Vector3 mousePosition = new Vector3(mousePos.x, mousePos.y, 0f);
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (hit.transform.gameObject.TryGetComponent(out ISelectable selectable))
            {
                selectable.Select();
            }
        }
    }
}
