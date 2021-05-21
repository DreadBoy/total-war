using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MinNumberOfUnits = 8;
    private const float UnitSize = 2f;

    [SerializeField] private EntityHighlight entityHighlightPrefab;
    [SerializeField] private Soldier soldierPrefab;

    private int _numberOfUnits;
    [Range(MinNumberOfUnits, 64)] public int numberOfUnits;

    private Vector3 _destination = Vector3.zero;
    private Vector3 _forward = Vector3.zero;

    public Vector3 Destination
    {
        get => _destination;
        set
        {
            _forward = Vector3.ProjectOnPlane(value - _destination, Vector3.up).normalized;
            _destination = value;

            StartCoroutine(RepositionSoldiers());
            StartCoroutine(HighlightDestination());
        }
    }

    public Vector3 Forward => _forward;

    private List<Soldier> _soldiers = new List<Soldier>();

    private void Reset()
    {
        numberOfUnits = 8;
        _soldiers = new List<Soldier>();
    }

    private void Awake()
    {
        _numberOfUnits = numberOfUnits;
        var t = transform;
        _destination = t.position;
        _forward = t.forward;
        SpawnSoldiers();
    }


    private void SpawnSoldiers()
    {
        var localPositions = new UnitPositions(_numberOfUnits, UnitSize).Line().ToList();
        for (var i = 0; i < _numberOfUnits; i++)
        {
            var globalPosition = ProjectOnTerrain(localPositions[i]);

            var soldier = Instantiate(soldierPrefab, transform);
            soldier.WarpTo(globalPosition);
            _soldiers.Add(soldier);
        }
    }

    public void KillSoldiers(int numberOfKilled)
    {
        var wasHighlighted = IsHighlighted;
        for (var i = 0; i < numberOfKilled; i++)
        {
            var index = Random.Range(0, _numberOfUnits);
            KillSoldier(index);
        }

        IsHighlighted = wasHighlighted;

        StartCoroutine(RepositionSoldiers());
    }

    private void KillSoldier(int index)
    {
        if (_numberOfUnits - 1 < MinNumberOfUnits) return;

        _numberOfUnits--;

        Destroy(_soldiers[index].gameObject);
        _soldiers.RemoveAt(index);
    }

    public bool IsHighlighted
    {
        get => _soldiers.Aggregate(false, (agg, soldier) => agg || soldier.IsHighlighted);
        set
        {
            foreach (var soldier in _soldiers)
                soldier.IsHighlighted = value;
        }
    }

    public static bool AreHighlighted
    {
        get => FindObjectsOfType<Unit>().Aggregate(false, (agg, unit) => agg || unit.IsHighlighted);
        set
        {
            foreach (var unit in FindObjectsOfType<Unit>())
                unit.IsHighlighted = value;
        }
    }

    [CanBeNull] public static Unit Highlighted => FindObjectsOfType<Unit>().FirstOrDefault(u => u.IsHighlighted);


    IEnumerator RepositionSoldiers()
    {
        yield return new WaitForEndOfFrame();
        var localPositions = new UnitPositions(_numberOfUnits, UnitSize).Line().ToList();
        for (var i = 0; i < _numberOfUnits; i++)
        {
            _soldiers[i].GoTo(ProjectOnTerrain(localPositions[i]));
        }
    }

    IEnumerator HighlightDestination()
    {
        var globalPositions = new UnitPositions(_numberOfUnits, UnitSize).Line().Select(ProjectOnTerrain).ToList();
        var highlights = globalPositions.Select(p =>
        {
            var h = Instantiate(entityHighlightPrefab, transform);
            h.transform.position = p;
            h.IsHighlighted = true;
            return h;
        }).ToList();
        yield return new WaitForSeconds(0.2f);
        foreach (var highlight in highlights)
            Destroy(highlight.gameObject);
    }

    /// <summary>
    /// Projects local position to terrain and returns global position. Also accounts for unit's heading/forward
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    private Vector3 ProjectOnTerrain(Vector3 localPosition)
    {
        var t = transform;
        var oldRotation = t.rotation;
        t.rotation = Quaternion.LookRotation(_forward, Vector3.up);
        var globalPosition = transform.TransformPoint(localPosition) + Destination;
        t.rotation = oldRotation;
        
        var result = new RaycastHit[1];
        var mask = LayerMask.GetMask("Terrain");
        var hits = Physics.RaycastNonAlloc(globalPosition + Vector3.up * 500, Vector3.down, result, 1000, mask);
        return hits == 1 ? result[0].point : globalPosition;
    }
}