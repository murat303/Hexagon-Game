using System;
using UnityEngine;

namespace Hexagon
{
    /// <summary>
    /// odd-q offset coordinate system
    /// </summary>
    public struct CoordinatesOffset
    {
        public int Row { get; }
        public int Col { get; }

        public CoordinatesOffset(int col, int row)
        {
            Row = row;
            Col = col;
        }

        public CoordinatesCube ToCube()
        {
            var x = Col;
            var z = Row - (Col + (Col & 1)) / 2;
            var y = -x - z;
            return new CoordinatesCube(x, y, z);
        }

        public Vector2 ToUnity()
        {
            return ToCube().ToUnity();
        }

        public bool Equals(CoordinatesOffset other)
        {
            return Row == other.Row && Col == other.Col;
        }
    }
}
