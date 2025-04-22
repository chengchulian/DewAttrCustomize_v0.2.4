using DG.Tweening;
using UnityEngine;

public class UI_Title_SplashView_ViewAnimator : MonoBehaviour
{
	private void OnEnable()
	{
		base.transform.DOKill(complete: true);
		base.transform.localScale = Vector3.one * 0.9f;
		base.transform.DOScale(1f, 0.3f);
	}
}
