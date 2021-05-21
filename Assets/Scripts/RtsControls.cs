using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PlayerInput))]
public class RtsControls : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    private void Reset()
    {
        camera = GetComponent<Camera>();
    }

    public void OnClick(InputValue value)
    {
        var clicked = (int) value.Get<float>();
        switch (clicked)
        {
            case 0:
            {
                var pos = Mouse.current.position.ReadValue();
                var ray = camera.ScreenPointToRay(pos);
                var hits = new RaycastHit[1];
                var mask = LayerMask.GetMask("Soldier");
                var hitCount = Physics.RaycastNonAlloc(ray, hits, 1000, mask);
                if (hitCount == 1)
                {
                    var soldier = hits[0].transform.GetComponent<Soldier>();
                    if (soldier != null)
                        soldier.Unit.IsHighlighted = true;
                }
                else
                {
                    Unit.AreHighlighted = false;
                }

                break;
            }
        }
    }

    public void OnRightClick(InputValue value)
    {
        var clicked = (int) value.Get<float>();
        switch (clicked)
        {
            case 0:
            {
                var unit = Unit.Highlighted;
                if (unit == null) break;

                var pos = Mouse.current.position.ReadValue();
                var ray = camera.ScreenPointToRay(pos);
                var hits = new RaycastHit[1];
                var mask = LayerMask.GetMask("Terrain");
                var hitCount = Physics.RaycastNonAlloc(ray, hits, 1000, mask);
                if (hitCount == 1)
                {
                    unit.Destination = hits[0].point;
                }

                break;
            }
        }
    }
}