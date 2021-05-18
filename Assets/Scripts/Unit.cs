using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    private const int MinNumberOfUnits = 8;
    private const float UnitSize = 2f;

    [SerializeField] private UnitHighlight unitHighlightPrefab;
    [SerializeField] private Soldier soldierPrefab;
    [Range(MinNumberOfUnits, 64)] public int numberOfUnits;
    private int _numberOfUnits;

    [SerializeField] private List<Soldier> soldiers;
    [SerializeField] private List<UnitHighlight> unitHighlights;

    private void Reset()
    {
        numberOfUnits = 8;
        soldiers = new List<Soldier>();
        unitHighlights = new List<UnitHighlight>();
    }

    private void Awake()
    {
        _numberOfUnits = numberOfUnits;
        SpawnSoldiers();
        StartCoroutine(RepositionSoldiers());
    }


    private void SpawnSoldiers()
    {
        var positions = new UnitPositions(_numberOfUnits, UnitSize).Line();
        for (var i = 0; i < _numberOfUnits; i++)
            SpawnSoldier(positions[i]);
    }

    private void SpawnSoldier(Vector3 localPosition)
    {
        var soldier = Instantiate(soldierPrefab, transform);
        soldier.transform.localPosition = localPosition;
        soldiers.Add(soldier);

        var unitHighlight = Instantiate(unitHighlightPrefab, transform);
        unitHighlight.transform.localPosition = localPosition;
        unitHighlight.Shown(true);
        unitHighlights.Add(unitHighlight);
    }

    public void KillSoldiers(int numberOfKilled)
    {
        for (int i = 0; i < numberOfKilled; i++)
        {
            var index = Random.Range(0, _numberOfUnits);
            KillSoldier(index);
        }
    }

    public void KillSoldier(int index)
    {
        if (_numberOfUnits - 1 < MinNumberOfUnits) return;

        _numberOfUnits--;

        Destroy(soldiers[index].gameObject);
        soldiers.RemoveAt(index);

        Destroy(unitHighlights[index].gameObject);
        unitHighlights.RemoveAt(index);
        var positions = new UnitPositions(_numberOfUnits, UnitSize).Line();
        for (var i = 0; i < positions.Length; i++)
            unitHighlights[i].transform.localPosition = positions[i];
    }

    IEnumerator RepositionSoldiers()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            for (var i = 0; i < soldiers.Count; i++)
            {
                var localPositions = unitHighlights.Select(h => h.transform.localPosition).ToArray();
                soldiers[i].CheckYourPosition(localPositions, UnitSize);
            }

            yield return new WaitForSeconds(1);
        }
    }
}