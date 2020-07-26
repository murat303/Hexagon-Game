using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities.Audio;

namespace Hexagon
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Header("Settings")]
        public List<Color> Colors;
        public int BombLife;
        public int BombScoreCheck;
        public int HexagonScore;
        public int ColumnCount;
        public int RowCount;
        public float GridSize;

        [Header("Prefabs")]
        public GameObject Hexagon;
        public GameObject Bomb;
        public ParticleSystem ParticleFx;
        public TextMeshPro ScoreText;

        [Header("Sound")]
        public Sound soundRotate;
        public Sound soundExplode;
        public Sound soundBombExplode;
        public Sound soundBombTimer;

        [Header("Debug")]
        public bool logsEnabled = true;
    }

}