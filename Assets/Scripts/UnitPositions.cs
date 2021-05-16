using System;
using System.Linq;
using UnityEngine;

public class UnitPositions
{
    private int Count { get; }

    public UnitPositions(int count)
    {
        Count = count;
    }

    public Vector3[] Line()
    {
        var origin = (Count + Count % 2) * -Vector3.right;
        return Enumerable
            .Range(0, Count)
            .Select(i => origin + 2 * i * Vector3.right)
            .ToArray();

    }

    public void Square()
    {
    }

    private Comparison<Vector3> SortByNearest(Vector3 origin)
    {
        return (a, b) => (int) Mathf.Ceil(Vector3.Distance(origin, a) - Vector3.Distance(origin, b));
    }
}