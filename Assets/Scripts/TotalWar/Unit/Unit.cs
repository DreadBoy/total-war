using UnityEngine;

namespace TotalWar.Unit
{
    public class Unit : MonoBehaviour
    {
        public UnitDescriptor UnitDescriptor { get; private set; }

        public Entity.Entity[] Entities => GetComponentsInChildren<Entity.Entity>();

        public void Initialize(UnitDescriptor unitDescriptor, Vector3 position)
        {
            UnitDescriptor = unitDescriptor;
            transform.position = position;
        }

        /// <summary>
        /// Projects local position to terrain and returns global position. Also accounts for unit's heading/forward
        /// </summary>
        /// <param name="localPosition"></param>
        /// <param name="unitPosition"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public Vector3 ProjectOnTerrain(Vector3 localPosition, Vector3 unitPosition, Vector3 forward)
        {
            var t = transform;
            var oldRotation = t.rotation;
            // TODO Make this more efficient
            if (forward != Vector3.zero)
                t.rotation = Quaternion.LookRotation(forward, Vector3.up);
            var globalPosition = transform.TransformPoint(localPosition) + unitPosition;
            if (t.rotation != oldRotation)
                t.rotation = oldRotation;

            var result = new RaycastHit[1];
            var mask = LayerMask.GetMask("Terrain");
            var hits = Physics.RaycastNonAlloc(globalPosition + Vector3.up * 500, Vector3.down, result, 1000, mask);
            return hits == 1 ? result[0].point : globalPosition;
        }
    }
}