using System;
using Lean.Touch;
using UnityEngine;

namespace Hexagon
{
    public enum Swipe
    {
        Left = 0,
        Right = 1
    }

    public class InputManager : Singleton<InputManager>
    {
        public event Action<Swipe, Vector2> Swiped;
        public event Action<Vector3> Tapped;

        public LeanFingerSwipe rightSwipe, leftSwipe;
        public LeanFingerTap tap;

        void Start()
        {
            Input.multiTouchEnabled = false;
        }

        public void OnRightSwipe(LeanFinger finger)
        {
            Logger.Log("Right swipe");

            Swiped?.Invoke(Swipe.Right, finger.GetStartWorldPosition(-10));
        }

        public void OnLeftSwipe(LeanFinger finger)
        {
            Logger.Log("Left swipe");

            Swiped?.Invoke(Swipe.Left, finger.GetStartWorldPosition(-10));
        }

        public void OnTap(LeanFinger finger)
        {
            var screenPos = finger.ScreenPosition;
            var worldPos = finger.GetWorldPosition(10, Camera.current);

            Logger.Log("WorldPos: " + worldPos.ToString());

            if (Physics2D.OverlapPoint(worldPos))
            {
                Tapped?.Invoke(worldPos);
            }
        }
    }
}
