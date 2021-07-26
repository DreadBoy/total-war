using UnityEngine;

namespace TotalWar.Player
{
    [RequireComponent(typeof(Player))]
    public class SpawnUnits : MonoBehaviour
    {
        [SerializeField] private Unit.Unit unitPrefab;
        [SerializeField] private UnitDescriptor unitDescriptor;

        private void Awake()
        {
            if (unitPrefab == null)
                throw new System.ArgumentNullException("unitPrefab");
            if (unitDescriptor == null)
                throw new System.ArgumentNullException("unitDescriptor");
        }

        public void Start()
        {
            var unit = Instantiate(unitPrefab, transform);
            unit.Initialize(unitDescriptor, Vector3.zero);
        }
    }
}