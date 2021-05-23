using UnityEngine;

[CreateAssetMenu(fileName = "UnitDescriptor", menuName = "Descriptors/UnitDescriptor", order = 1)]
public class UnitDescriptor : ScriptableObject
{
    public int minNumberOfEntities = 8;
    public int defaultNumberOfEntities = 32;
    public float entitySize = 2f;

    public EntityHighlight entityHighlightPrefab;
    public Entity entityPrefab;
}