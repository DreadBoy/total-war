using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class RtsCamera : MonoBehaviour
{
    private Coroutine _move;

    public void OnMove(InputValue value)
    {
        var move = value.Get<Vector2>();

        if (_move != null)
        {
            StopCoroutine(_move);
            _move = null;
        }

        if (move != Vector2.zero)
            _move = StartCoroutine(Move(move));
    }

    public void OnScrollWheel(InputValue value)
    {
        var scroll = value.Get<Vector2>().y;
        var sign =  Mathf.Sign(scroll);

        if (scroll != 0)
            transform.Translate(Vector3.forward * sign);

        // if (use && look != Vector2.zero)
        // {
        //     transform.Rotate(Vector3.up, look.x * 0.3f, Space.World);
        //     transform.Rotate(Vector3.right, -look.y * 0.3f);
        // }
        //
        // if (fire && look != Vector2.zero)
        // {
        //     var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        //     var right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        //
        //     var translate = (forward * -look.y + right * -look.x) * 0.1f;
        //     transform.Translate(translate, Space.World);
        // }
    }

    IEnumerator Move(Vector2 move)
    {
        for (;;)
        {
            var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
            var distance = transform.position.y;

            var speed = Mathf.Max((distance - 4) / 16f, 0.05f);
            
            var translate = (forward * move.y + right * move.x) * speed;
            transform.Translate(translate, Space.World);
            yield return new WaitForFixedUpdate();
        }
    }
}

// 4 => 0.1
// 16 => 1

// 12x => 10X