using System.Collections;
using UnityEngine;
using VolumetricFogAndMist2;

public class DarkCaveCutSceneFogFadeOut : MonoBehaviour
{
	private VolumetricFog _fog;

	private void Awake()
	{
		_fog = GetComponent<VolumetricFog>();
		_fog.profile.albedo.a = 1f;
		_fog.profile.noiseStrength = 0f;
	}

	public void FogFadeStart(float duration)
	{
		StartCoroutine(FadeOutCoroutine(duration));
	}

	private IEnumerator FadeOutCoroutine(float duration)
	{
		float time = 0f;
		while (time < duration)
		{
			_fog.profile.noiseStrength = Mathf.Lerp(0f, 1.8f, time / duration);
			_fog.profile.albedo.a = Mathf.Lerp(1f, 0f, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
		_fog.profile.albedo.a = 0f;
	}
}
