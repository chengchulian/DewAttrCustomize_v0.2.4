using UnityEngine;

namespace HighlightPlus.Demos;

public class HitFxDemo : MonoBehaviour
{
	public AudioClip hitSound;

	private void Update()
	{
		if (InputProxy.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(InputProxy.mousePosition), out var hitInfo))
		{
			HighlightEffect effect = hitInfo.collider.GetComponent<HighlightEffect>();
			if (!(effect == null))
			{
				AudioSource.PlayClipAtPoint(hitSound, hitInfo.point);
				effect.HitFX(hitInfo.point);
			}
		}
	}
}
