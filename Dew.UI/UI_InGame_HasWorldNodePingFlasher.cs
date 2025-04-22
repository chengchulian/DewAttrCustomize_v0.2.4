using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_HasWorldNodePingFlasher : MonoBehaviour
{
	public Image flasherImage;

	private void Start()
	{
		InGameUIManager instance = InGameUIManager.instance;
		instance.onHasWorldNodePingChanged = (Action<bool>)Delegate.Combine(instance.onHasWorldNodePingChanged, new Action<bool>(OnHasWorldNodePingChanged));
	}

	private void OnHasWorldNodePingChanged(bool obj)
	{
		if (!obj)
		{
			StopAllCoroutines();
			this.DOKill(complete: true);
		}
		else
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			while (true)
			{
				if (InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.None)
				{
					Boop();
				}
				yield return new WaitForSecondsRealtime(1f);
			}
		}
	}

	private void Boop()
	{
		base.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f);
		Color c = new Color(0.2f, 0.75f, 1f);
		flasherImage.color = c;
		c.a = 0f;
		flasherImage.DOColor(c, 1f);
	}
}
