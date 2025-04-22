using UnityEngine;
using UnityEngine.UI;

public abstract class UI_GenericBar : LogicBehaviour
{
	public Image fillImage;

	public Image downDeltaImage;

	public Image upDeltaImage;

	public float deltaDampTime = 0.1f;

	private float _downDeltaCv;

	private float _upDeltaCv;

	protected abstract float GetFillAmount();

	protected virtual void Start()
	{
		UpdateFillAmounts();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		UpdateFillAmounts();
	}

	protected virtual void UpdateFillAmounts()
	{
		float amount = GetFillAmount();
		if (amount < fillImage.fillAmount)
		{
			fillImage.fillAmount = amount;
		}
		else if (amount > fillImage.fillAmount)
		{
			if (upDeltaImage == null)
			{
				fillImage.fillAmount = amount;
			}
			else
			{
				fillImage.fillAmount = Mathf.SmoothDamp(fillImage.fillAmount, amount, ref _upDeltaCv, deltaDampTime);
			}
		}
		if (downDeltaImage != null)
		{
			downDeltaImage.fillAmount = Mathf.SmoothDamp(downDeltaImage.fillAmount, fillImage.fillAmount, ref _downDeltaCv, deltaDampTime);
		}
		if (upDeltaImage != null)
		{
			upDeltaImage.fillAmount = amount;
		}
	}
}
