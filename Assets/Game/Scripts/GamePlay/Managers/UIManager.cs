using TMPro;
using UnityEngine;

namespace Hexagon
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject screenGamePlay;
        public GameObject screenGameOver;

        public TextMeshProUGUI txtScore;
        public TextMeshProUGUI txtMoves;
        public TextMeshProUGUI txtGameOverScore;

        int moveCount;

        void Start()
        {
            GameManager.Instance.OnScoreChanged += OnScoreChanged;
        }

        void OnScoreChanged(int score)
        {
            txtScore.text = score.ToString();
            txtGameOverScore.text = txtScore.text;
        }

        public void AddMoveCount()
        {
            moveCount++;
            txtMoves.text = moveCount.ToString();
        }

        public void ShowGameOver()
        {
            screenGamePlay.SetActive(false);
            screenGameOver.SetActive(true);
        }
    }
}
