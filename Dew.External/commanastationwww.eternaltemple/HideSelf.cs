using System.Collections;
using UnityEngine;

namespace commanastationwww.eternaltemple;

public class HideSelf : MonoBehaviour
{
	private int collisionEntriesCounter;

	private Material mat;

	private float currentAlpha;

	public bool hideable = true;

	private float hidingSpeed = 0.3f;

	private float minAlpha = 0.03f;

	private float maxAlpha = 0.2f;

	private void Start()
	{
		mat = GetComponent<Renderer>().material;
		currentAlpha = mat.GetFloat("_Cutoff");
	}

	private IEnumerator unideInterpolation()
	{
		while (currentAlpha > minAlpha)
		{
			currentAlpha -= hidingSpeed * Time.deltaTime;
			mat.SetFloat("_Cutoff", currentAlpha);
			yield return null;
		}
	}

	private IEnumerator hideInterpolation()
	{
		while (currentAlpha < maxAlpha)
		{
			currentAlpha += hidingSpeed * Time.deltaTime;
			mat.SetFloat("_Cutoff", currentAlpha);
			yield return null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (hideable)
		{
			if (other.gameObject.tag == "MainCamera")
			{
				collisionEntriesCounter++;
			}
			StopAllCoroutines();
			StartCoroutine(hideInterpolation());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (hideable)
		{
			if (other.gameObject.tag == "MainCamera")
			{
				collisionEntriesCounter--;
			}
			if (collisionEntriesCounter <= 0)
			{
				StopAllCoroutines();
				StartCoroutine(unideInterpolation());
				collisionEntriesCounter = 0;
			}
		}
	}

	private void Update()
	{
	}
}
