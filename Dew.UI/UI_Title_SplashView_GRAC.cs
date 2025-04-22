using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title_SplashView_GRAC : MonoBehaviour
{
	public Image timerFill;

	private void OnEnable()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			float startTime = Time.unscaledTime;
			float duration = 3.5f;
			while (Time.unscaledTime - startTime < duration)
			{
				if (timerFill != null)
				{
					timerFill.fillAmount = 1f - (Time.unscaledTime - startTime) / duration;
				}
				yield return null;
			}
			GetComponentInParent<UI_Title_SplashView>().ShowNext();
		}
	}
}
