using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_GRACPeriodicWarning : MonoBehaviour
{
	private int _playHours;

	private void Start()
	{
		if (!UI_Title_SplashView.IsUserInSouthKorea())
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnEnable()
	{
		TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
		CanvasGroup cg = GetComponent<CanvasGroup>();
		int intervalSeconds = 3600;
		cg.alpha = 0f;
		text.text = "";
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (true)
			{
				yield return new WaitForSecondsRealtime(intervalSeconds - 4);
				_playHours++;
				text.text = $"과도한 게임이용은 정상적인 일상생활에 지장을 줄 수 있습니다.\n셰이프 오브 드림즈를 플레이하신 지 {_playHours}시간이 경과하였습니다.";
				cg.DOFade(1f, 1f);
				yield return new WaitForSecondsRealtime(4f);
				cg.DOFade(0f, 1f);
			}
		}
	}
}
