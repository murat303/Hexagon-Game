using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Utilities.Audio;

namespace Hexagon
{
    public enum RotationDirection
    {
        Clockwise = 0,
        ClockwiseOpposite = 1
    }

    public class RotateController : MonoBehaviour
    {
        Transform highlighterTransform;

        void Start()
        {
            highlighterTransform = Highlighter.Instance.transform;
        }

        public IEnumerator Rotate(RotationDirection direction)
        {
            for (int i = 0; i < 3; i++)
            {
                SoundManager.Instance.PlaySound(GameManager.Instance.settings.soundRotate);

                if(direction == RotationDirection.Clockwise)
                    yield return RotateClockwise();
                else
                    yield return RotateClockwiseOpposite();

                // check for matches
                yield return MatchController.Instance.CheckMatches();

                if (MatchController.Instance.MatchFound)
                {
                    Logger.Log("Match found! Rotation cancelled");
                    yield break;
                }
            }
        }

        IEnumerator RotateClockwise()
        {
            var hexagon1 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon1];
            var hexagon2 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon2];
            var hexagon3 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon3];

            SetSelectedHexagons(highlighterTransform, 0);

            // Hex1 --> Hex2
            SetHexagon(hexagon1, GameManager.Instance.SelectedGroup.Hexagon2);

            // Hex2 --> Hex3
            SetHexagon(hexagon2, GameManager.Instance.SelectedGroup.Hexagon3);

            // Hex3 --> Hex1
            SetHexagon(hexagon3, GameManager.Instance.SelectedGroup.Hexagon1);

            yield return StartCoroutine(Highlighter.Instance.Rotate(-120, 0.175f));
            SetSelectedHexagons(GridBuilder.Instance.transform, -1);
        }

        IEnumerator RotateClockwiseOpposite()
        {
            var hexagon1 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon1];
            var hexagon2 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon2];
            var hexagon3 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon3];

            SetSelectedHexagons(highlighterTransform, 0);

            // Hex1 --> Hex3
            SetHexagon(hexagon1, GameManager.Instance.SelectedGroup.Hexagon3);

            // Hex3 --> Hex2
            SetHexagon(hexagon3, GameManager.Instance.SelectedGroup.Hexagon2);

            // Hex2 --> Hex1
            SetHexagon(hexagon2, GameManager.Instance.SelectedGroup.Hexagon1);

            yield return StartCoroutine(Highlighter.Instance.Rotate(120, 0.175f));
            SetSelectedHexagons(GridBuilder.Instance.transform, -1);
        }

        void SetSelectedHexagons(Transform parent, int sortingOrder)
        {
            var hexagon1 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon1];
            var hexagon2 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon2];
            var hexagon3 = GridDatabase.Instance[GameManager.Instance.SelectedGroup.Hexagon3];

            hexagon1.transform.parent = parent;
            hexagon2.transform.parent = parent;
            hexagon3.transform.parent = parent;

            hexagon1.GetComponent<Hexagon>().SetSortingOrder(sortingOrder);
            hexagon2.GetComponent<Hexagon>().SetSortingOrder(sortingOrder);
            hexagon3.GetComponent<Hexagon>().SetSortingOrder(sortingOrder);
        }

        void SetHexagon(GameObject hexagon, CoordinatesOffset coords)
        {
            GridDatabase.Instance[coords] = hexagon;
        }
    }
}
