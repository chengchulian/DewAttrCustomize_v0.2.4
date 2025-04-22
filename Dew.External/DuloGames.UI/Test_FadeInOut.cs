using System;
using DuloGames.UI.Tweens;
using UnityEngine;

namespace DuloGames.UI;

public class Test_FadeInOut : MonoBehaviour
{
	[SerializeField]
	private float m_Duration = 4f;

	[SerializeField]
	private TweenEasing m_Easing = TweenEasing.InOutQuint;

	private CanvasGroup m_Group;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	protected Test_FadeInOut()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected void OnEnable()
	{
		m_Group = GetComponent<CanvasGroup>();
		if (m_Group == null)
		{
			m_Group = base.gameObject.AddComponent<CanvasGroup>();
		}
		StartAlphaTween(0f, m_Duration, ignoreTimeScale: true);
	}

	private void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
	{
		if (!(m_Group == null))
		{
			float currentAlpha = m_Group.alpha;
			if (!currentAlpha.Equals(targetAlpha))
			{
				FloatTween floatTween = default(FloatTween);
				floatTween.duration = duration;
				floatTween.startFloat = currentAlpha;
				floatTween.targetFloat = targetAlpha;
				FloatTween floatTween2 = floatTween;
				floatTween2.AddOnChangedCallback(SetAlpha);
				floatTween2.AddOnFinishCallback(OnTweenFinished);
				floatTween2.ignoreTimeScale = ignoreTimeScale;
				floatTween2.easing = m_Easing;
				m_FloatTweenRunner.StartTween(floatTween2);
			}
		}
	}

	private void SetAlpha(float alpha)
	{
		if (!(m_Group == null))
		{
			m_Group.alpha = alpha;
		}
	}

	protected virtual void OnTweenFinished()
	{
		if (!(m_Group == null))
		{
			StartAlphaTween((m_Group.alpha == 1f) ? 0f : 1f, m_Duration, ignoreTimeScale: true);
		}
	}
}
