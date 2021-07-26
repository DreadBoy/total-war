using System.Collections;
using System.Linq;
using TotalWar.Entity;
using UnityEngine;

namespace TotalWar.Unit
{
    [RequireComponent(typeof(Unit))]
    public class MoveUnit : MonoBehaviour
    {
        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }


        private Vector3 _destination;
        private Vector3 _forward;

        public Vector3 Destination
        {
            get => _destination;
            set
            {
                _forward = Vector3.ProjectOnPlane(value - _destination, Vector3.up).normalized;
                _destination = value;
            
                StartCoroutine(RepositionSoldiers());
                StartCoroutine(HighlightDestination());
            }
        }

        public Vector3 Forward => _forward;

        public IEnumerator RepositionSoldiers()
        {
            yield return new WaitForEndOfFrame();
            var localPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line().ToList();
            for (var i = 0; i < _unit.Entities.Length; i++)
            {
                _unit.Entities[i].GetComponent<MoveEntity>().GoTo(ProjectOnTerrain(localPositions[i]));
            }
        }

        IEnumerator HighlightDestination()
        {
            var globalPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line()
                .Select(ProjectOnTerrain).ToList();
            var highlights = globalPositions.Select(p =>
            {
                var h = Instantiate(_unit.UnitDescriptor.entityHighlightPrefab, transform);
                h.transform.position = p;
                h.IsHighlighted = true;
                return h;
            }).ToList();
            yield return new WaitForSeconds(0.2f);
            foreach (var highlight in highlights)
                Destroy(highlight.gameObject);
        }

        /// <summary>
        /// Projects local position to terrain and returns global position. Also accounts for unit's heading/forward
        /// </summary>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public Vector3 ProjectOnTerrain(Vector3 localPosition)
        {
            var t = transform;
            var oldRotation = t.rotation;
            // TODO Make this more efficient
            if (_forward != Vector3.zero)
                t.rotation = Quaternion.LookRotation(_forward, Vector3.up);
            var globalPosition = transform.TransformPoint(localPosition) + Destination;
            if (t.rotation != oldRotation)
                t.rotation = oldRotation;

            var result = new RaycastHit[1];
            var mask = LayerMask.GetMask("Terrain");
            var hits = Physics.RaycastNonAlloc(globalPosition + Vector3.up * 500, Vector3.down, result, 1000, mask);
            return hits == 1 ? result[0].point : globalPosition;
        }
    }
}