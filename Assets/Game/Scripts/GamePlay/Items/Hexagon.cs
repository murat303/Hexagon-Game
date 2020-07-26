using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Hexagon
{
    public class Hexagon : MonoBehaviour
    {
        public Color Color { get; set; }
        public SpriteRenderer spriteRenderer { get; set; }

        public void SetColor(Color color)
        {
            Color = color;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material.color = color;
        }

        public void SetSortingOrder(int value)
        {
            spriteRenderer.sortingOrder = value;
        }

        public void Explode()
        {
            var originalScale = transform.localScale;
            var targetScale = originalScale * 0.01f;

            GameManager.Instance.ShowParticleFx(transform.position, Color);

            transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public IEnumerator MoveTo(Vector3 target, float time)
        {
            GetComponent<Rigidbody2D>().DOMove(target, time).SetEase(Ease.Linear);

            yield return new WaitForSeconds(time);
        }
    }
}
