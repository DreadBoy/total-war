using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MinNumberOfUnits = 8;
    private const float UnitSize = 2f;

    [SerializeField] private UnitHighlight unitHighlightPrefab;
    [SerializeField] private Soldier soldierPrefab;
    [Range(MinNumberOfUnits, 64)] public int numberOfUnits;
    private int _numberOfUnits;

    [SerializeField] private List<Vector3> localPositions;
    private List<Soldier> _soldiers = new List<Soldier>();

    private void Reset()
    {
        numberOfUnits = 8;
        _soldiers = new List<Soldier>();
    }

    private void Awake()
    {
        _numberOfUnits = numberOfUnits;
        SpawnSoldiers();
    }


    private void SpawnSoldiers()
    {
        localPositions = new UnitPositions(_numberOfUnits, UnitSize).Line().ToList();
        for (var i = 0; i < _numberOfUnits; i++)
            SpawnSoldier(localPositions[i]);
    }

    private void SpawnSoldier(Vector3 localPosition)
    {
        var globalPosition = ProjectOnTerrain(localPosition);

        var soldier = Instantiate(soldierPrefab, transform);
        soldier.WarpTo(globalPosition);
        _soldiers.Add(soldier);
    }

    public void KillSoldiers(int numberOfKilled)
    {
        for (var i = 0; i < numberOfKilled; i++)
        {
            var index = Random.Range(0, _numberOfUnits);
            KillSoldier(index);
        }

        StartCoroutine(RepositionSoldiers());
    }

    private void KillSoldier(int index)
    {
        if (_numberOfUnits - 1 < MinNumberOfUnits) return;

        _numberOfUnits--;

        Destroy(_soldiers[index].gameObject);
        _soldiers.RemoveAt(index);
        localPositions.RemoveAt(index);
    }

    IEnumerator RepositionSoldiers()
    {
        yield return new WaitForEndOfFrame();
        localPositions = new UnitPositions(_numberOfUnits, UnitSize).Line().ToList();
        for (var i = 0; i < _numberOfUnits; i++)
        {
            _soldiers[i].GoTo(ProjectOnTerrain(localPositions[i]));
        }
    }
    
    /// <summary>
    /// Projects local position to terrain and returns global position 
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    private Vector3 ProjectOnTerrain(Vector3 localPosition)
    {
        var globalPosition = transform.TransformPoint(localPosition);
        var result = new RaycastHit[1];
        var mask = LayerMask.GetMask("Terrain");
        var hits = Physics.RaycastNonAlloc(globalPosition + Vector3.up * 1000, Vector3.down, result, 1000, mask);
        return hits == 1 ? result[0].point : globalPosition;
    }
}