using System.Collections.Generic;
using UnityEngine;

namespace Hexagon
{
    public class GridBuilder : Singleton<GridBuilder>
    {
        void Start()
        {
            var columnCount = GameManager.Instance.settings.ColumnCount;
            var rowCount = GameManager.Instance.settings.RowCount;

            CreateGrid(columnCount, rowCount);

            foreach (var hexagonGroup in GetHexagonGroups(columnCount, rowCount))
            {
                GameManager.Instance.AddGroup(hexagonGroup);
            }
        }

        void CreateGrid(int columnCount, int rowCount)
        {
            for (var col = 0; col < columnCount; col++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    var performColorCheck = (col > 0) && (row > 0);

                    var offsetCoordinates = new CoordinatesOffset(col, row);
                    var hex = CreateHexagon(offsetCoordinates, false, performColorCheck);
                    hex.name = $"({col}, {row})";
                    GridDatabase.Instance[offsetCoordinates] = hex;
                }
            }
        }

        IEnumerable<Group> GetHexagonGroups(int columnCount, int rowCount)
        {
            for (int col = 0; col < columnCount - 1; col += 2)
            for (int row = 0; row < rowCount - 1; row++)
            {
                var hexagon1 = new CoordinatesOffset(col, row);
                var hexagon2 = new CoordinatesOffset(col + 1, row + 1);
                var hexagon3 = new CoordinatesOffset(col + 1, row);

                yield return new Group(hexagon1, hexagon2, hexagon3, GroupOrientation.TwoRight);
            }

            for (int col = 0; col < columnCount - 1; col += 2)
            for (int row = 0; row < rowCount - 1; row++)
            {
                var hexagon1 = new CoordinatesOffset(col, row);
                var hexagon2 = new CoordinatesOffset(col, row + 1);
                var hexagon3 = new CoordinatesOffset(col + 1, row + 1);

                yield return new Group(hexagon1, hexagon2, hexagon3, GroupOrientation.TwoLeft);
            }

            for (int col = 1; col < columnCount - 1; col += 2)
            for (int row = 1; row < rowCount; row++)
            {
                var hexagon1 = new CoordinatesOffset(col, row);
                var hexagon2 = new CoordinatesOffset(col + 1, row);
                var hexagon3 = new CoordinatesOffset(col + 1, row - 1);

                yield return new Group(hexagon1, hexagon2, hexagon3, GroupOrientation.TwoRight);
            }

            for (int col = 1; col < columnCount - 1; col += 2)
            for (int row = 0; row < rowCount - 1; row++)
            {
                var hexagon1 = new CoordinatesOffset(col, row);
                var hexagon2 = new CoordinatesOffset(col, row + 1);
                var hexagon3 = new CoordinatesOffset(col + 1, row);

                yield return new Group(hexagon1, hexagon2, hexagon3, GroupOrientation.TwoLeft);
            }
        }

        public GameObject CreateHexagon(CoordinatesOffset offsetCoordinates, bool isBomb, bool performColorCheck)
        {
            var prefab = isBomb ? GameManager.Instance.settings.Bomb : GameManager.Instance.settings.Hexagon;

            var newHexagon = Instantiate(prefab, transform);
            newHexagon.transform.position = offsetCoordinates.ToUnity();
            newHexagon.name = $"({offsetCoordinates.Col}, {offsetCoordinates.Row})";

            var colour = GetColor(performColorCheck, offsetCoordinates);
            newHexagon.GetComponent<Hexagon>().SetColor(colour);

            return newHexagon;
        }

        Color GetColor(bool performColorCheck, CoordinatesOffset offsetCoordinates)
        {
            if (performColorCheck)
            {
                var checkA = new CoordinatesOffset(offsetCoordinates.Col - 1, offsetCoordinates.Row);
                var checkB = new CoordinatesOffset(offsetCoordinates.Col - 1, offsetCoordinates.Row - 1);
                var checkC = new CoordinatesOffset(offsetCoordinates.Col, offsetCoordinates.Row - 1);

                var colA = GridDatabase.Instance[checkA].GetComponent<Hexagon>().Color;
                var colB = GridDatabase.Instance[checkB].GetComponent<Hexagon>().Color;
                var colC = GridDatabase.Instance[checkC].GetComponent<Hexagon>().Color;

                if (colA == colB)
                {
                    return GameLogic.RandomColor(except: colA);
                }

                if (colB == colC)
                {
                    return GameLogic.RandomColor(except: colB);
                }

                if (colA == colC)
                {
                    return GameLogic.RandomColor(except: colA);
                }
            }

            return GameLogic.RandomColor();
        }
    }
}
