using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Audio;

namespace Hexagon
{
    public class GameManager : Singleton<GameManager>
    {
        public GameSettings settings;

        bool isSelectionActive = false;
        bool rotationActive;
        RotateController rotateController;

        [HideInInspector]
        public List<Group> hexagonGroups = new List<Group>();
        public Group SelectedGroup { get; set; }

        public int Score { get; set; }

        //Events
        public event Action<bool> OnRotationCompleted;
        public event Action<int> OnScoreChanged;
        public event Action OnBombIsSpawnable;

        void Start()
        {
            rotateController = GetComponent<RotateController>();

            InputManager.Instance.OnSwiped += InputManagerOnSwiped;
            InputManager.Instance.OnTapped += InputManagerOnTapped;
        }

        public void Restart()
        {
            SceneManager.LoadScene("Start");
        }

        #region Visuals
        public void ShowParticleFx(Vector3 position, Color color)
        {
            var particleFx = Instantiate(settings.ParticleFx, transform);
            particleFx.transform.position = position;
            ParticleSystem.MainModule ma = particleFx.main;
            ma.startColor = color;
            particleFx.Play();
            Destroy(particleFx.gameObject, ma.startLifetime.constant);
        }
        public void ShowScore(Vector3 position, int count)
        {
            var scoreText = Instantiate(settings.ScoreText, transform);
            scoreText.transform.position = position;

            scoreText.text = (count * settings.HexagonScore).ToString();

            scoreText.transform.DOLocalMoveY(1, 2f).SetRelative(true).OnComplete(() => { Destroy(scoreText.gameObject); });
        } 
        #endregion

        #region Inputs
        void InputManagerOnTapped(Vector3 worldPosition)
        {
            if (rotationActive) return;

            var closestGroup = FindClosestGroup(worldPosition);
            Logger.Log("Center of closest group: " + closestGroup.Center);

            SelectGroup(closestGroup);
        }

        void SelectGroup(Group group)
        {
            SelectedGroup = group;
            isSelectionActive = true;

            Highlighter.Instance.Highlight(group);
        }

        void InputManagerOnSwiped(Swipe swipeDirection, Vector2 swipeOrigin)
        {
            if (rotationActive) return;

            if (!isSelectionActive) return;
            StartCoroutine(Rotate(swipeDirection, swipeOrigin));
        } 

        IEnumerator Rotate(Swipe swipeDirection, Vector2 swipeOrigin)
        {
            rotationActive = true;
            var rotationDirection = GetRotateDirection(swipeDirection, SelectedGroup.Center, swipeOrigin);

            yield return rotateController.Rotate(rotationDirection);

            if(MatchController.Instance.MatchFound)
                UIManager.Instance.AddMoveCount();

            OnRotationCompleted?.Invoke(MatchController.Instance.MatchFound);

            Highlighter.Instance.Activate();
            rotationActive = false;
        }

        RotationDirection GetRotateDirection(Swipe swipeDirection, Vector2 selectionPosition, Vector2 swipeOrigin)
        {
            var swipeOriginIsAboveSelection = swipeOrigin.y > selectionPosition.y;

            if(swipeDirection == Swipe.Left)
                return swipeOriginIsAboveSelection ? RotationDirection.ClockwiseOpposite : RotationDirection.Clockwise;
            else
                return swipeOriginIsAboveSelection ? RotationDirection.Clockwise : RotationDirection.ClockwiseOpposite;
        }
        #endregion

        #region Groups
        public void AddGroup(Group group)
        {
            hexagonGroups.Add(group);
        }

        public Group FindClosestGroup(Vector2 point)
        {
            float previousDistance = 999;
            Group selectedGroup = hexagonGroups.First();
            foreach (var g in hexagonGroups)
            {
                var distance = Vector3.SqrMagnitude(point - g.Center);
                if (distance < previousDistance)
                {
                    previousDistance = distance;
                    selectedGroup = g;
                }
            }

            return selectedGroup;
        } 
        #endregion

        #region Score
        public void AddScore()
        {
            Score += settings.HexagonScore;

            if (Score > 0 && Score % settings.BombScoreCheck == 0)
            {
                OnBombIsSpawnable?.Invoke();
            }

            OnScoreChanged?.Invoke(Score);
        }
        #endregion
    }
}
