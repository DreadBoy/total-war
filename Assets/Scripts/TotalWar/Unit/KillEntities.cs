using UnityEngine;

namespace TotalWar.Unit
{
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(HighlightUnit))]
    [RequireComponent(typeof(MoveUnit))]
    public class KillEntities : MonoBehaviour
    {
        private Unit _unit;
        private HighlightUnit _highlightUnit;
        private MoveUnit _moveUnit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _highlightUnit = GetComponent<HighlightUnit>();
            _moveUnit = GetComponent<MoveUnit>();
        }

        private Entity.Entity[] Entities => GetComponentsInChildren<Entity.Entity>();

        public void KillSoldiers(int numberOfKilled)
        {
            var wasHighlighted = _highlightUnit.IsHighlighted;
            for (var i = 0; i < numberOfKilled; i++)
            {
                var index = Random.Range(0, _unit.Entities.Length);
                KillSoldier(index);
            }

            _highlightUnit.IsHighlighted = wasHighlighted;

            StartCoroutine(_moveUnit.RepositionSoldiers());
        }

        private void KillSoldier(int index)
        {
            if (_unit.Entities.Length - 1 < _unit.UnitDescriptor.minNumberOfEntities) return;

            Destroy(Entities[index].gameObject);
        }
    }
}