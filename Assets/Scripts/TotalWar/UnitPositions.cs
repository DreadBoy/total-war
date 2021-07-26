using System;
using System.Linq;
using UnityEngine;

namespace TotalWar
{
    public class UnitPositions
    {
        private int Count { get; }
        private float UnitSize { get; }

        public UnitPositions(int count, float unitSize)
        {
            Count = count;
            UnitSize = unitSize;
        }

        /// <summary>
        /// Three deep line of soldiers
        /// </summary>
        /// <returns></returns>
        public Vector3[] Line()
        {
            const int depth = 3;

            var missing = Count % depth;
            if (missing > 0) missing = depth - missing;
            var perLine = (Count + missing) / depth;

            var origin = (perLine - perLine % 2) / 2 * UnitSize * -Vector3.right;
            if (missing == 0)
            {
                return Enumerable
                    .Range(0, depth)
                    .SelectMany(line =>
                        Enumerable
                            .Range(0, perLine)
                            .Select(i => origin + line * UnitSize * Vector3.back + i * UnitSize * Vector3.right)
                    )
                    .ToArray();
            }

            var lastLine = Count % perLine;
            var originLastLine = (lastLine - lastLine % 2) / 2 * UnitSize * -Vector3.right;

            return Enumerable
                .Range(0, depth - 1)
                .SelectMany(line =>
                    Enumerable
                        .Range(0, perLine)
                        .Select(i => origin + line * UnitSize * Vector3.back + i * UnitSize * Vector3.right)
                )
                .Concat(
                    Enumerable
                        .Range(0, lastLine)
                        .Select(i => originLastLine + (depth - 1) * UnitSize * Vector3.back + i * UnitSize * Vector3.right)
                )
                .ToArray();
        }

        public Comparison<Vector3> SortByNearest(Vector3 origin)
        {
            return (a, b) => (int) Mathf.Ceil(Vector3.Distance(origin, a) - Vector3.Distance(origin, b));
        }
    }
}