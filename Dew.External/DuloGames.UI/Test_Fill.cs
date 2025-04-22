using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Test_Fill : MonoBehaviour
{
	[SerializeField]
	private Image imageComponent;

	[SerializeField]
	private float Duration = 5f;

	[SerializeField]
	private TweenEasing Easing = TweenEasing.InOutQuint;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	protected Test_Fill()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected void OnEnable()
	{
		if (!(imageComponent == null))
		{
			StartTween(0f, imageComponent.fillAmount * Duration);
		}
	}

	protected void SetFillAmount(float amount)
	{
		if (!(imageComponent == null))
		{
			imageComponent.fillAmount = amount;
		}
	}

	protected void OnTweenFinished()
	{
		if (!(imageComponent == null))
		{
			StartTween((imageComponent.fillAmount == 0f) ? 1f : 0f, Duration);
		}
	}

	protected void StartTween(float targetFloat, float duration)
	{
		if (!(imageComponent == null))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = imageComponent.fillAmount;
			floatTween.targetFloat = targetFloat;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetFillAmount);
			floatTween2.AddOnFinishCallback(OnTweenFinished);
			floatTween2.ignoreTimeScale = true;
			floatTween2.easing = Easing;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}
}
