using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

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

        public IEnumerator Rotate(RotationDirection direction)
        {
            yield return null;
        }
    }
}
