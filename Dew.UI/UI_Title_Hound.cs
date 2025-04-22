using System;
using DG.Tweening;
using UnityEngine;

public class UI_Title_Hound : MonoBehaviour
{
	public UI_Title_LizardSmoothie logo;

	public Vector3 normalScale;

	public Vector3 flippedScale;

	public Vector3 punchScale;

	public float punchDuration;

	private void Start()
	{
		if (logo != null)
		{
			UI_Title_LizardSmoothie uI_Title_LizardSmoothie = logo;
			uI_Title_LizardSmoothie.onClicked = (Action<bool>)Delegate.Combine(uI_Title_LizardSmoothie.onClicked, new Action<bool>(OnClicked));
		}
	}

	private void OnClicked(bool isFlipped)
	{
		base.transform.DOKill();
		base.transform.localScale = (isFlipped ? flippedScale : normalScale);
		base.transform.DOPunchScale(punchScale, punchDuration);
	}
}
