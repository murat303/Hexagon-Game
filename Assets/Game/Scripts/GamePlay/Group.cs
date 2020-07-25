using UnityEngine;

namespace Hexagon
{
    public enum GroupOrientation
    {
        TwoLeft = 0,
        TwoRight = 1
    }

    public struct Group
    {
        public CoordinatesOffset Hexagon1 { get; }
        public CoordinatesOffset Hexagon2 { get; }
        public CoordinatesOffset Hexagon3 { get; }

        public GroupOrientation Orientation { get; }

        public Group(CoordinatesOffset hexagon1, CoordinatesOffset hexagon2, CoordinatesOffset hexagon3, GroupOrientation orientation)
        {
            Hexagon1 = hexagon1;
            Hexagon2 = hexagon2;
            Hexagon3 = hexagon3;
            Orientation = orientation;
        }

        public Vector2 Center { get { return (Hexagon1.ToUnity() + Hexagon2.ToUnity() + Hexagon3.ToUnity()) / 3f; } }
    }
}
