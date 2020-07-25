using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Hexagon
{
    public class Hexagon : MonoBehaviour
    {
        public Color Color { get; set; }

        public void SetColor(Color color)
        {
            Color = color;
            GetComponentInChildren<Renderer>().material.color = color;
        }

        public void Explode()
        {
            transform.DOScale(0.01f, 0.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public IEnumerator MoveTo(Vector3 target, float time)
        {
            GetComponent<Rigidbody2D>().DOMove(target, time);
            yield return new WaitForSeconds(time);
        }
    }
}
