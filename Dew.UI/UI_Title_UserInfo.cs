using System.Collections;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;

public class UI_Title_UserInfo : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (string.IsNullOrEmpty(AnalyticsService.Instance.GetAnalyticsUserID()))
			{
				UpdateText();
				yield return new WaitForSecondsRealtime(1f);
			}
			UpdateText();
		}
	}

	private void UpdateText()
	{
		GetComponent<TextMeshProUGUI>().text = "UA:" + AnalyticsService.Instance.GetAnalyticsUserID();
	}
}
