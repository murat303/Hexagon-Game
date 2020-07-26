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
        public event Action<Swipe, Vector2> OnSwiped;
        public event Action<Vector3> OnTapped;

        public LeanFingerSwipe rightSwipe, leftSwipe;
        public LeanFingerTap tap;

        public string gameLayer;

        void Start()
        {
            Input.multiTouchEnabled = false;
        }

        public void OnRightSwipe(LeanFinger finger)
        {
            Logger.Log("Right swipe");
            OnSwiped?.Invoke(Swipe.Right, finger.GetStartWorldPosition(-10));
        }

        public void OnLeftSwipe(LeanFinger finger)
        {
            Logger.Log("Left swipe");
            OnSwiped?.Invoke(Swipe.Left, finger.GetStartWorldPosition(-10));
        }

        public void OnTap(LeanFinger finger)
        {
            var screenPos = finger.ScreenPosition;
            var worldPos = finger.GetWorldPosition(10, Camera.current);

            Logger.Log("WorldPos: " + worldPos.ToString());

            int mask = 1 << LayerMask.NameToLayer(gameLayer);
            if (Physics2D.OverlapPoint(worldPos, mask))
            {
                OnTapped?.Invoke(worldPos);
            }
        }
    }
}
