using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(DecalProjector))]
public class UnitHighlight : MonoBehaviour
{
    private DecalProjector Projector => GetComponent<DecalProjector>();

    public UnitHighlight Shown(bool shown)
    {
        Projector.enabled = shown;
        return this;
    }
}