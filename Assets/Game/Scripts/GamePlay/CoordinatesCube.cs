using UnityEngine;

namespace Hexagon
{
    /// <summary>
    /// hexagonal cube coordinate system.
    /// </summary>
    public struct CoordinatesCube
    {
        readonly Vector3Int _values;

        public int Q => _values.x;
        public int S => _values.y;
        public int R => _values.z;

        public int X => Q;
        public int Y => S;
        public int Z => R;

        public CoordinatesCube(int q, int s, int r)
        {
            _values = new Vector3Int(q, s, r);
        }

        public Vector2 ToUnity() //hex to pixel
        {
            var size = 1;

            var x = size * (3f / 2 * Q);
            var y = size * (Mathf.Sqrt(3) / 2 * Q + Mathf.Sqrt(3) * R);
            return new Vector2(x, y);
        }
    }
}
