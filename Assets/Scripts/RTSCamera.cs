using UnityEngine;

[RequireComponent(typeof(InputActions))]
public class RTSCamera : MonoBehaviour
{
    [SerializeField]
    private InputActions inputActions;

    private void Reset()
    {
        inputActions = GetComponent<InputActions>();
    }

    private void FixedUpdate()
    {
        var move = inputActions.Move;
        var look = inputActions.Look;
        var fire = inputActions.Fire;
        var use = inputActions.Use;
        
        if (move != Vector2.zero)
        {
            var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

            var translate = (forward * move.y + right * move.x) * 4;
            transform.Translate(translate, Space.World);
        }

        if (use && look != Vector2.zero)
        {
            transform.Rotate(Vector3.up, look.x * 0.3f, Space.World);
            transform.Rotate(Vector3.right, -look.y * 0.3f);
        }

        if (fire && look != Vector2.zero)
        {
            var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

            var translate = (forward * -look.y + right * -look.x) * 0.1f;
            transform.Translate(translate, Space.World);
        }

    }
}