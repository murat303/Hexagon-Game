using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Hexagon
{
    public class Highlighter : Singleton<Highlighter>
    {
        GameObject highlight;

        void Start()
        {
            highlight = transform.GetChild(0).gameObject;
        }

        public void Highlight(Group group)
        {
            Activate();
            transform.position = group.Center;
            SetOrientation(group.Orientation);
        }

        void SetOrientation(GroupOrientation orientation)
        {
            var zRot = orientation == GroupOrientation.TwoRight ? 0 : 180;
            transform.rotation = Quaternion.AngleAxis(zRot, new Vector3(0, 0, 1));
        }

        public IEnumerator Rotate(float rotate, float time)
        {
            transform.DORotate(new Vector3(0, 0, rotate), time, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
            yield return new WaitForSeconds(time);
        }

        public void Disable()
        {
            highlight.SetActive(false);
        } 
        public void Activate()
        {
            highlight.SetActive(true);
        }
    }
}
