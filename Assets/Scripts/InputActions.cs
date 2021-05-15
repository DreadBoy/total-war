using UnityEngine;
using UnityEngine.InputSystem;

public class InputActions : MonoBehaviour
{
    public InputActionAsset asset;
    private InputAction _move;
    private InputAction _look;
    private InputAction _fire;
    private InputAction _use;

    private void OnEnable()
    {
        asset.Enable();
    }

    private void OnDisable()
    {
        asset.Disable();
    }

    private void Awake()
    {
        _move = asset.FindAction("Move");
        _look = asset.FindAction("Look");
        _fire = asset.FindAction("Fire");
        _use = asset.FindAction("Use");
    }

    public Vector2 Move => _move.ReadValue<Vector2>();
    public Vector2 Look => _look.ReadValue<Vector2>();
    public bool Fire => _fire.ReadValue<float>() > 0.5f;
    public bool Use => _use.ReadValue<float>() > 0.5f;
}