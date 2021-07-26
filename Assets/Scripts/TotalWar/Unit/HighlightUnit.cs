using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace TotalWar.Unit
{
    [RequireComponent(typeof(Unit))]
    public class HighlightUnit : MonoBehaviour
    {
        private Unit _unit;

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


        IEnumerator HighlightDestination()
        {
            var globalPositions = new UnitPositions(_unit.Entities.Length, _unit.UnitDescriptor.entitySize).Line()
                .Select(lp => _unit.ProjectOnTerrain(lp, transform.position)).ToList();
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
    }
}