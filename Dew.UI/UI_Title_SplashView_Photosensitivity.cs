using UnityEngine;

public class UI_Title_SplashView_Photosensitivity : MonoBehaviour
{
	public Transform balloon;

	private void Update()
	{
		balloon.localRotation = Quaternion.Euler(-30f, Mathf.Sin(Time.unscaledTime * 2.5f) * 15f + -10f, Mathf.Sin(Time.unscaledTime * 2.5f) * 3f);
	}
}
