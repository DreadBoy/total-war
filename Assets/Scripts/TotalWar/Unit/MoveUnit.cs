using System.Collections;
using System.Linq;
using TotalWar.Entity;
using UnityEngine;

namespace TotalWar.Unit
{
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(HighlightUnit))]
    public class MoveUnit : MonoBehaviour
    {
        private Unit _unit;
        private HighlightUnit _highlightUnit;
        private Transform _transform;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _highlightUnit = GetComponent<HighlightUnit>();
            _transform = transform;
        }

        public void GoTo(Vector3 destination, Vector3 forward)
        {
            forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;

            StartCoroutine(DoRepositionSoldiers(destination, forward));
            _highlightUnit.HighlightDestination(destination, forward);
        }

        public void RepositionSoldiers()
        {
            
            StartCoroutine(DoRepositionSoldiers(_transform.position, _transform.forward));
        }

        private IEnumerator DoRepositionSoldiers(Vector3 destination, Vector3 forward)
        {
            yield return new WaitForEndOfFrame();
            var localPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line()
                .ToList();
            for (var i = 0; i < _unit.Entities.Length; i++)
            {
                _unit.Entities[i].GetComponent<MoveEntity>().GoTo(
                    _unit.ProjectOnTerrain(localPositions[i], destination, forward),
                    forward
                );
            }
        }
    }
}