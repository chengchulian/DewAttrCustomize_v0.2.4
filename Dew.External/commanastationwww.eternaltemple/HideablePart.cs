using System.Collections;
using UnityEngine;

namespace commanastationwww.eternaltemple;

public class HideablePart : MonoBehaviour
{
	private Material mat;

	private float currentAlpha;

	private float fadeOutSpeed = 0.3f;

	private float minAlpha = 0.03f;

	private float maxAlpha = 0.2f;

	private void Start()
	{
		mat = GetComponent<Renderer>().material;
		currentAlpha = mat.GetFloat("_Cutoff");
	}

	public void hide()
	{
		StopAllCoroutines();
		StartCoroutine(hideInterpolation());
	}

	public void unhide()
	{
		StopAllCoroutines();
		StartCoroutine(unhideInterpolation());
	}

	private IEnumerator unhideInterpolation()
	{
		while (currentAlpha > minAlpha)
		{
			currentAlpha -= fadeOutSpeed * Time.deltaTime;
			mat.SetFloat("_Cutoff", currentAlpha);
			yield return null;
		}
	}

	private IEnumerator hideInterpolation()
	{
		while (currentAlpha < maxAlpha)
		{
			currentAlpha += fadeOutSpeed * Time.deltaTime;
			mat.SetFloat("_Cutoff", currentAlpha);
			yield return null;
		}
	}

	private void Update()
	{
	}
}
