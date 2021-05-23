using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Unit unitPrefab;
    [SerializeField] private UnitDescriptor unitDescriptor;
    private void Awake()
    {
        if(unitDescriptor == null)
            Debug.LogError("unitDescriptor is null, assign correct unit description in Player component!");
        Instantiate(unitPrefab).Init(this, unitDescriptor);
    }
}
