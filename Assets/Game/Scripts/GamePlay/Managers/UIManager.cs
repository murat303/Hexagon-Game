using TMPro;
using UnityEngine;

namespace Hexagon
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI txtScore;
        public TextMeshProUGUI txtMoves;

        void Start()
        {
            GameManager.Instance.OnScoreChanged += OnScoreChanged;
        }

        void OnScoreChanged(int score)
        {
            txtScore.text = score.ToString();
        }
    }
}
