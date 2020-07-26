using System;
using TMPro;
using UnityEngine;
using Utilities.Audio;

namespace Hexagon
{
    public class Bomb : MonoBehaviour
    {
        public int Lives { get; set; }
        TextMeshPro txtLive;

        void Start()
        {
            Lives = GameManager.Instance.settings.BombLife;
            txtLive = GetComponentInChildren<TextMeshPro>();
            txtLive.text = Lives.ToString();
            GameManager.Instance.OnRotationCompleted += OnRotationCompletedR;
        }

        void OnDisable()
        {
            if(GameManager.Instance)
                GameManager.Instance.OnRotationCompleted -= OnRotationCompletedR;
        }

        void OnRotationCompletedR(bool matchFound)
        {
            if(matchFound)
            {
                Lives--;
                txtLive.text = Lives.ToString();

                if (Lives <= 0)
                {
                    SoundManager.Instance.PlaySound(GameManager.Instance.settings.soundBombExplode);
                    UIManager.Instance.ShowGameOver();
                }
                else
                {
                    SoundManager.Instance.PlaySound(GameManager.Instance.settings.soundBombTimer);
                }
            }
        }
    }
}
