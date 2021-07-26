using UnityEngine;
using UnityEngine.AI;

namespace TotalWar.Entity
{
    public class Entity : MonoBehaviour
    {
        private Transform _transform;
        private EntityHighlight _entityHighlight;

        private void Awake()
        {
            _transform = transform;
            _entityHighlight = GetComponentInChildren<EntityHighlight>();
        }

        public Unit.Unit Unit => GetComponentInParent<Unit.Unit>();

        public void Initialize(Vector3 position, Vector3 forward)
        {
            _transform.position = position;
            _transform.forward = forward;
        }

        public bool IsHighlighted
        {
            get => _entityHighlight.IsHighlighted;
            set => _entityHighlight.IsHighlighted = value;
        }
    }
}