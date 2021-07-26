using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace TotalWar
{
    [RequireComponent(typeof(DecalProjector))]
    public class EntityHighlight : MonoBehaviour
    {
        private DecalProjector Projector => GetComponent<DecalProjector>();

        public bool IsHighlighted
        {
            get => Projector.enabled;
            set => Projector.enabled = value;
        }
    }
}