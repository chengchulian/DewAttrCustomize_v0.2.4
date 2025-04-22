using System.Collections;
using UnityEngine;

public class UI_WiggleScale : MonoBehaviour
{
	public float timeScale = 5f;

	public float timeOffset;

	public float offset = 1f;

	public float magnitude = 0.1f;

	private void OnEnable()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (true)
			{
				base.transform.localScale = Vector3.one * (Mathf.Sin(Time.unscaledTime * timeScale + timeOffset) * magnitude + offset);
				yield return null;
			}
		}
	}
}
