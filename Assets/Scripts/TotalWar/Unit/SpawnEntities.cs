using System.Linq;
using UnityEngine;

namespace TotalWar.Unit
{
    [RequireComponent(typeof(Unit))]
    public class SpawnEntities : MonoBehaviour
    {
        private Unit _unit;
        private Transform _transform;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _transform = transform;
        }

        public void Start()
        {
            var descriptor = _unit.UnitDescriptor;
            var localPositions = new UnitPositions(descriptor.defaultNumberOfEntities, descriptor.entitySize).Line()
                .ToList();

            for (var i = 0; i < localPositions.Count; i++)
            {
                var globalPosition = _unit.ProjectOnTerrain(localPositions[i], _transform.position);

                var entity = Instantiate(descriptor.entityPrefab, transform);
                entity.Initialize(globalPosition, _transform.forward);
            }
        }
    }
}