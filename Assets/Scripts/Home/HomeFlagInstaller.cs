using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeFlagInstaller : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private float _rayDistance = 100f;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _positionDifference;

    private Flag _installedFlag;
    private Flag _choosingPlaceFlag;
    private PlayerInput _playerInput;

    public event Action Installed;

    public Flag InstalledFlag => _installedFlag;
    public Camera Camera => _camera;

    private void Awake()
    {
        _installedFlag = Instantiate(_flagPrefab);
        _installedFlag.gameObject.SetActive(false);
        _choosingPlaceFlag = Instantiate(_flagPrefab);
        _choosingPlaceFlag.gameObject.SetActive(false);
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Mouse.RightButtonClick.performed += CancelChoosing;
        _playerInput.Mouse.LeftButtonClick.performed += InstallFlag;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Mouse.RightButtonClick.performed -= CancelChoosing;
        _playerInput.Mouse.LeftButtonClick.performed -= InstallFlag;
    }

    private void Update()
    {
        if (TryGetFlagMousePosition(out Vector3 position))
        {
            _choosingPlaceFlag.transform.position = position;
        }
    }

    private bool TryGetFlagMousePosition(out Vector3 position)
    {
        Vector2 mousePos = _playerInput.Mouse.Position.ReadValue<Vector2>();
        Vector3 mousePosition = new Vector3(mousePos.x, mousePos.y, 0f);
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (hit.transform.gameObject.TryGetComponent(out Ground ground))
            {
                position = hit.point + _positionDifference;
                
                return true;
            }
        }

        position = Vector3.zero;

        return false;
    }

    public void ChoseFlagPlace()
    {
        _choosingPlaceFlag.gameObject.SetActive(true);
    }

    private void CancelChoosing(InputAction.CallbackContext context)
    {
        _choosingPlaceFlag.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void InstallFlag(InputAction.CallbackContext context)
    {
        if (TryGetFlagMousePosition(out Vector3 position))
        {
            _installedFlag.gameObject.SetActive(true);
            _installedFlag.Initialize(position);
            Installed?.Invoke();
            CancelChoosing(context);
        }
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }
}
