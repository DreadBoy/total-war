using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Soldier soldierPrefab;
    [Range(8, 64)] public int numberOfUnits;


    public Soldier[] soldiers;
    private UnitHighlight[] _unitHighlights;

    private void Reset()
    {
        numberOfUnits = 8;
        soldiers = new Soldier[0];
        _unitHighlights = new UnitHighlight[0];
    }

    private void Awake()
    {
        SpawnSoldiers();
    }


    private void SpawnSoldiers()
    {
        var soldiers = new List<Soldier>();
        var positions = new UnitPositions(numberOfUnits).Line();

        for (var i = 0; i < numberOfUnits; i++)
        {
            var soldier = Instantiate(soldierPrefab, transform);
            soldier.transform.localPosition = positions[i];
            soldiers.Add(soldier);
        }

        this.soldiers = soldiers.ToArray();
    }
}