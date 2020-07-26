using UnityEngine;

namespace Hexagon
{
    public class GridDatabase : Singleton<GridDatabase>
    {
        /// <summary>
        /// Dim0 = col
        /// Dim1 = row
        /// [col, row] => hexagon
        /// </summary>
        public GameObject[,] HexagonGrid { get; set; }

        void Awake()
        {
            HexagonGrid = new GameObject[GameManager.Instance.settings.ColumnCount, GameManager.Instance.settings.RowCount];
        }

        public GameObject this[CoordinatesOffset offsetCoordinates]
        {
            get => HexagonGrid[offsetCoordinates.Col, offsetCoordinates.Row];
            set => HexagonGrid[offsetCoordinates.Col, offsetCoordinates.Row] = value;
        }

        public (GameObject hex1, GameObject hex2, GameObject hex3) this[Group group] =>
        (
            this[group.Hexagon1],
            this[group.Hexagon2],
            this[group.Hexagon3]
        );

        public void MarkAsDestroyed(CoordinatesOffset coords)
        {
            this[coords] = null;
        }

        /// <summary>
        /// Swaps the [col, rowA] with [col, rowB].
        /// </summary>
        public static void Swap(int col, int rowA, int rowB)
        {
            // temp <- b
            var temp = Instance.HexagonGrid[col, rowB];

            // b <- a
            Instance.HexagonGrid[col, rowB] = Instance.HexagonGrid[col, rowA];

            // a <- temp
            Instance.HexagonGrid[col, rowA] = temp;
        }
    }
}
