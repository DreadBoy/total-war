using TotalWar.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TotalWar
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(PlayerInput))]
    public class RtsControls : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        private bool _rightClick;
        private bool _previewUnitMove;

        private void Reset()
        {
            camera = GetComponent<Camera>();
        }

        public void OnClick(InputValue value)
        {
            var isDown = (int) value.Get<float>() == 1;
            switch (isDown)
            {
                case true:
                {
                    var pos = Mouse.current.position.ReadValue();
                    var ray = camera.ScreenPointToRay(pos);
                    var hits = new RaycastHit[1];
                    var mask = LayerMask.GetMask("Soldier");
                    var hitCount = Physics.RaycastNonAlloc(ray, hits, 1000, mask);
                    if (hitCount == 1)
                    {
                        var soldier = hits[0].transform.GetComponent<Entity.Entity>();
                        if (soldier != null)
                            soldier.Unit.GetComponent<HighlightUnit>().IsHighlighted = true;
                    }
                    else
                    {
                        HighlightUnit.AreHighlighted = false;
                        HighlightUnit.CancelPreview();
                    }

                    break;
                }
            }
        }

        public void OnRightClick(InputValue value)
        {
            _rightClick = (int) value.Get<float>() == 1;
            _previewUnitMove = false;

            var pos = Mouse.current.position.ReadValue();
            var ray = camera.ScreenPointToRay(pos);
            var terrainHits = new RaycastHit[1];
            var terrainHitsCount = Physics.RaycastNonAlloc(
                ray,
                terrainHits,
                1000,
                LayerMask.GetMask("Terrain")
            );

            switch (_rightClick)
            {
                case true:
                {
                    var unit = HighlightUnit.Highlighted;
                    if (unit == null || terrainHitsCount == 0)
                        break;


                    var highlightUnit = unit.GetComponent<HighlightUnit>();
                    highlightUnit.StartPreview(terrainHits[0].point);
                    _previewUnitMove = true;

                    break;
                }
                case false:
                {
                    var unit = HighlightUnit.Highlighted;
                    if (unit == null || terrainHitsCount == 0)
                    {
                        HighlightUnit.CancelPreview();
                        break;
                    }

                    var highlightUnit = unit.GetComponent<HighlightUnit>();
                    var shouldMove = highlightUnit.FinishPreview(
                        terrainHits[0].point,
                        out var destination,
                        out var forward
                    );

                    if (!shouldMove) break;

                    var moveUnit = unit.GetComponent<MoveUnit>();
                    moveUnit.GoTo(destination, forward);
                    break;
                }
            }
        }

        public void OnPoint()
        {
            if (_previewUnitMove)
            {
                ContinuePreviewUnitMove();
            }
        }

        private void ContinuePreviewUnitMove()
        {
            var pos = Mouse.current.position.ReadValue();
            var ray = camera.ScreenPointToRay(pos);
            var terrainHits = new RaycastHit[1];
            var terrainHitsCount = Physics.RaycastNonAlloc(
                ray,
                terrainHits,
                1000,
                LayerMask.GetMask("Terrain")
            );

            var unit = HighlightUnit.Highlighted;
            if (unit == null || terrainHitsCount == 0) return;

            var highlightUnit = unit.GetComponent<HighlightUnit>();
            highlightUnit.UpdatePreview(terrainHits[0].point);
        }
    }
}