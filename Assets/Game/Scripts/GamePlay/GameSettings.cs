using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexagon
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Header("Settings")]
        public List<Color> Colors;
        public int BombLife;
        public int BombScore;
        public int HexagonScore;
        public int ColumnCount;
        public int RowCount;
        public float Size;

        [Header("Prefabs")]
        public GameObject Hexagon;
        public GameObject Bomb;
    }

}