using System.Linq;
using UnityEngine;

namespace Hexagon
{
    public class GameLogic
    {
        public static Color RandomColor()
        {
            var colors = GameManager.Instance.settings.Colors;
            int index = Random.Range(0, colors.Count);
            return colors[index];
        }

        public static Color RandomColor(Color except)
        {
            var colors = GameManager.Instance.settings.Colors;
            var filtered = colors.Except(new[] { except }).ToArray();
            int index = Random.Range(0, filtered.Length);
            return filtered[index];
        }

        public static int CountForColor(Group group, Color foundColor)
        {
            int count = 0;

            var (ac, bc, cc) = GetColors(group);

            if (ac == foundColor) count++;
            if (bc == foundColor) count++;
            if (cc == foundColor) count++;

            return count;
        }

        public static (Color hex1Color, Color hex2Color, Color hex3Color) GetColors(Group group)
        {
            var (a, b, c) = GridDatabase.Instance[group];
            var (ah, bh, ch) = (a.GetComponent<Hexagon>(), b.GetComponent<Hexagon>(), c.GetComponent<Hexagon>());
            return (ah.Color, bh.Color, ch.Color);
        }

        public static bool Contains(Group g, CoordinatesOffset oc)
        {
            return g.Hexagon1.Equals(oc) || g.Hexagon2.Equals(oc) || g.Hexagon3.Equals(oc);
        }

        public static bool IsSameColor(Hexagon a, Hexagon b, Hexagon c)
        {
            var ac = a.Color;
            var bc = b.Color;
            var cc = c.Color;

            return ac == bc && bc == cc && ac == cc;
        }

        static Vector2 centerOffset;
        public static Vector2 CenterOffset 
        { 
            get 
            {
                if(centerOffset == Vector2.zero)
                {
                    var ColumnCount = GameManager.Instance.settings.ColumnCount;
                    var RowCount = GameManager.Instance.settings.RowCount;

                    centerOffset = CalculateCenterOffset(ColumnCount, RowCount);
                }
                return centerOffset;
            } 
        }

        static float HexHorizontalDistance { get { return (GameManager.Instance.settings.GridSize * 2) * 3f / 4f; } }
        static float HexVerticalDistance { get { return GameManager.Instance.settings.GridSize * Mathf.Sqrt(3); } }

        static Vector2 CalculateCenterOffset(int colCount, int rowCount)
        {
            var totalWidth = HexHorizontalDistance * (colCount - 1);
            var totalHeight = HexVerticalDistance * (rowCount - 1);

            if (colCount > 1 || colCount < -1)
            {
                totalHeight += HexVerticalDistance / 2;
            }

            return new Vector2(totalWidth / 2, totalHeight / 2);
        }
    }
}
