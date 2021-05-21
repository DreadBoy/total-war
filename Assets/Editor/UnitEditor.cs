using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor
{
    private Unit _unit;

    private void OnEnable()
    {
        _unit = target.GetComponent<Unit>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        if (GUILayout.Button("ToggleHighlight"))
            _unit.IsHighlighted = !_unit.IsHighlighted;
        serializedObject.ApplyModifiedProperties();
    }

}
