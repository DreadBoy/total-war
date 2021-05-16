using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitHighlight unitHighlightPrefab;
    [SerializeField] private Soldier soldierPrefab;
    [Range(8, 64)] public int numberOfUnits;


    [SerializeField] private Soldier[] soldiers;
    [SerializeField] private UnitHighlight[] unitHighlights;

    private void Reset()
    {
        numberOfUnits = 8;
        soldiers = new Soldier[0];
        unitHighlights = new UnitHighlight[0];
    }

    private void Awake()
    {
        SpawnSoldiers();
    }


    private void SpawnSoldiers()
    {
        var _soldiers = new List<Soldier>();
        var _unitHighlights = new List<UnitHighlight>();
        
        var positions = new UnitPositions(numberOfUnits).Line();

        for (var i = 0; i < numberOfUnits; i++)
        {
            var soldier = Instantiate(soldierPrefab, transform);
            soldier.transform.localPosition = positions[i];
            _soldiers.Add(soldier);

            var unitHighlight = Instantiate(unitHighlightPrefab, transform);
            unitHighlight.transform.localPosition = positions[i];
            unitHighlight.Shown(true);
            _unitHighlights.Add(unitHighlight);
        }

        soldiers = _soldiers.ToArray();
        unitHighlights = _unitHighlights.ToArray();
    }
}