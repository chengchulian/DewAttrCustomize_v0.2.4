using System;
using DG.Tweening;
using UnityEngine;

public class UI_InGame_StardustGroup : MonoBehaviour
{
	private CanvasGroup _cg;

	private void Start()
	{
		_cg = GetComponent<CanvasGroup>();
		_cg.alpha = 0f;
		_cg.blocksRaycasts = false;
		GameManager.CallOnReady(delegate
		{
			DewPlayer.local.ClientEvent_OnEarnStardust += new Action<int>(ClientEventOnEarnStardust);
		});
	}

	private void ClientEventOnEarnStardust(int obj)
	{
		_cg.DOKill(complete: true);
		_cg.alpha = 1f;
		_cg.blocksRaycasts = true;
		DOTween.Sequence().AppendInterval(3.5f).Append(_cg.DOFade(0f, 2f))
			.AppendCallback(delegate
			{
				_cg.blocksRaycasts = false;
			})
			.SetId(_cg);
	}
}
