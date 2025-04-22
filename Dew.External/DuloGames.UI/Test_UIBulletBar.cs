using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class Test_UIBulletBar : MonoBehaviour
{
	public enum TextVariant
	{
		Percent,
		Value,
		ValueMax
	}

	public UIBulletBar bar;

	public float Duration = 5f;

	public TweenEasing Easing = TweenEasing.InOutQuint;

	public Text m_Text;

	public TextVariant m_TextVariant;

	public int m_TextValue = 100;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	protected Test_UIBulletBar()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected void OnEnable()
	{
		if (!(bar == null))
		{
			StartTween(0f, bar.fillAmount * Duration);
		}
	}

	protected void SetFillAmount(float amount)
	{
		if (bar == null)
		{
			return;
		}
		bar.fillAmount = amount;
		if (m_Text != null)
		{
			if (m_TextVariant == TextVariant.Percent)
			{
				m_Text.text = Mathf.RoundToInt(amount * 100f) + "%";
			}
			else if (m_TextVariant == TextVariant.Value)
			{
				m_Text.text = Mathf.RoundToInt((float)m_TextValue * amount).ToString();
			}
			else if (m_TextVariant == TextVariant.ValueMax)
			{
				m_Text.text = Mathf.RoundToInt((float)m_TextValue * amount) + "/" + m_TextValue;
			}
		}
	}

	protected void OnTweenFinished()
	{
		if (!(bar == null))
		{
			StartTween((bar.fillAmount == 0f) ? 1f : 0f, Duration);
		}
	}

	protected void StartTween(float targetFloat, float duration)
	{
		if (!(bar == null))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = bar.fillAmount;
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
