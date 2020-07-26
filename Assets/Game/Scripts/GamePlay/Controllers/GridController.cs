using System.Collections;
using UnityEngine;

namespace Hexagon
{
    public class GridController : Singleton<GridController>
    {
        bool isBombSpawnable;

        void Start()
        {
            GameManager.Instance.OnBombIsSpawnable += OnBombIsSpawnable;
        }

        void OnBombIsSpawnable()
        {
            isBombSpawnable = true;
        }

        public IEnumerator ShiftAndFill()
        {
            for (int col = 0; col < GridDatabase.Instance.HexagonGrid.GetLength(0); col++)
            {
                // count empty cells
                int shiftCount = 0;

                for (int row = 0; row < GridDatabase.Instance.HexagonGrid.GetLength(1); row++)
                {
                    var hex = GridDatabase.Instance.HexagonGrid[col, row];

                    if (!hex)
                    {
                        shiftCount++;
                    }
                    else
                    {
                        if (shiftCount > 0)
                        {
                            StartCoroutine(Shift(col, row, shiftCount, hex));
                        }
                    }
                }

                var rowLength = GridDatabase.Instance.HexagonGrid.GetLength(1);
                var fillSpawnRow = rowLength + 2;

                for (var row = rowLength - shiftCount; row < rowLength; row++)
                {
                    StartCoroutine(Fill(col, row, fillSpawnRow));
                }
            }

            yield return null;
        }

        IEnumerator Fill(int col, int row, int fillSpawnRow)
        {
            var fillTarget = new CoordinatesOffset(col, row);

            var hex = GridBuilder.Instance.CreateHexagon(new CoordinatesOffset(col, fillSpawnRow), isBombSpawnable, false);

            if (isBombSpawnable) isBombSpawnable = false;

            GridDatabase.Instance[fillTarget] = hex;

            yield return hex.GetComponent<Hexagon>().MoveTo(fillTarget.ToUnity(), 0.5f);
        }

        static IEnumerator Shift(int col, int row, int shiftCount, GameObject hex)
        {
            GridDatabase.Swap(col, row, row - shiftCount);

            var newCoords = new CoordinatesOffset(col, row - shiftCount);

            yield return hex.GetComponent<Hexagon>().MoveTo(newCoords.ToUnity(), 0.5f);
        }
    }
}
