using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal;

namespace TotalWar.Unit
{
    [RequireComponent(typeof(Unit))]
    public class HighlightUnit : MonoBehaviour
    {
        private Unit _unit;
        
        [SerializeField]
        private float unitRotateThreshold = 1;
        private static Vector3? _preview;
        private static EntityHighlight[] _previewHighlight = new EntityHighlight[0];
        
        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public bool IsHighlighted
        {
            get => _unit.Entities.Aggregate(false, (agg, entity) => agg || entity.IsHighlighted);
            set
            {
                foreach (var soldier in _unit.Entities)
                    soldier.IsHighlighted = value;
            }
        }

        public static bool AreHighlighted
        {
            get => FindObjectsOfType<HighlightUnit>().Aggregate(false, (agg, unit) => agg || unit.IsHighlighted);
            set
            {
                foreach (var unit in FindObjectsOfType<HighlightUnit>())
                    unit.IsHighlighted = value;
            }
        }

        [CanBeNull]
        public static Unit Highlighted =>
            FindObjectsOfType<HighlightUnit>().FirstOrDefault(u => u.IsHighlighted)?.GetComponent<Unit>();

        public void HighlightDestination(Vector3 destination, Vector3 forward)
        {
            StartCoroutine(DoHighlightDestination(destination, forward));
        }

        private IEnumerator DoHighlightDestination(Vector3 destination, Vector3 forward)
        {
            var globalPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line()
                .Select(localPosition => _unit.ProjectOnTerrain(localPosition, destination, forward)).ToList();
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

        public void StartPreview(Vector3 begin)
        {
            _preview = begin;

            var forward = Vector3.ProjectOnPlane(begin - transform.position, Vector3.up);
            var globalPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line()
                .Select(localPosition => _unit.ProjectOnTerrain(localPosition, begin, forward)).ToList();
            _previewHighlight = globalPositions.Select(p =>
            {
                var h = Instantiate(_unit.UnitDescriptor.entityHighlightPrefab, transform);
                h.transform.position = p;
                h.IsHighlighted = true;
                return h;
            }).ToArray();
        }

        public void UpdatePreview(Vector3 update)
        {
            if (_preview == null) return;

            var forward = Vector3.ProjectOnPlane(update - _preview.Value, Vector3.up);
            if (forward.magnitude < unitRotateThreshold) return;

            var globalPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line()
                .Select(localPosition => _unit.ProjectOnTerrain(localPosition, _preview.Value, forward)).ToList();

            for (int i = 0; i < _previewHighlight.Length && i < globalPositions.Count; i++)
            {
                _previewHighlight[i].transform.position = globalPositions[i];
            }
        }

        public static void CancelPreview()
        {
            _preview = null;
            foreach (var highlight in _previewHighlight)
                Destroy(highlight.gameObject);
        }

        public bool FinishPreview(Vector3 end, out Vector3 destination, out Vector3 forward)
        {
            foreach (var highlight in _previewHighlight)
                Destroy(highlight.gameObject);

            if (_preview == null)
            {
                destination = default;
                forward = default;
                return false;
            }

            destination = _preview.Value;
            forward = Vector3.ProjectOnPlane(end - _preview.Value, Vector3.up);
            if (forward.magnitude < unitRotateThreshold)
                forward = Vector3.ProjectOnPlane(end - transform.position, Vector3.up);
            return true;
        }
    }
}