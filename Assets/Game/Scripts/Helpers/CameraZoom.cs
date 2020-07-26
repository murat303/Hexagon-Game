using UnityEngine;

namespace Hexagon
{
    public class CameraZoom : MonoBehaviour
	{
		public SpriteRenderer referenceSprite;

		void Start()
		{
			Camera.main.orthographicSize = referenceSprite.bounds.size.x * Screen.height / Screen.width * 0.5f;
			referenceSprite.gameObject.SetActive(false);
		}
	}
}
