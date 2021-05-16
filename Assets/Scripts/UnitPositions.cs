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

    /// <summary>
    /// Three deep line of soldiers
    /// </summary>
    /// <returns></returns>
    public Vector3[] Line()
    {
        const int depth = 3;
        const float unitSize = 2f;

        var missing = Count % depth;
        if (missing > 0) missing = depth - missing;
        var perLine = (Count + missing) / depth;
        var lastLine = Count % perLine;

        var origin = perLine * unitSize * -Vector3.right / 2;
        var originLastLine = lastLine * unitSize * -Vector3.right / 2;
        return Enumerable
            .Range(0, depth - 1)
            .SelectMany(line =>
                Enumerable
                    .Range(0, perLine)
                    .Select(i => origin + line * unitSize * Vector3.back + i * unitSize * Vector3.right)
            )
            .Concat(
                Enumerable
                    .Range(0, lastLine)
                    .Select(i => originLastLine + (depth - 1) * unitSize * Vector3.back + i * unitSize * Vector3.right)
            )
            .ToArray();
    }

    private Comparison<Vector3> SortByNearest(Vector3 origin)
    {
        return (a, b) => (int) Mathf.Ceil(Vector3.Distance(origin, a) - Vector3.Distance(origin, b));
    }
}