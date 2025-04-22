using System;
using DuloGames.UI.Tweens;
using UnityEngine;

namespace DuloGames.UI;

public class Test_CompassTester : MonoBehaviour
{
	[SerializeField]
	private float m_Duration = 4f;

	[SerializeField]
	private TweenEasing m_Easing;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	protected Test_CompassTester()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected void OnEnable()
	{
		StartTween(360f, m_Duration, ignoreTimeScale: true);
	}

	private void StartTween(float targetRotation, float duration, bool ignoreTimeScale)
	{
		float currentRotation = base.transform.eulerAngles.y;
		if (!currentRotation.Equals(targetRotation))
		{
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startFloat = currentRotation;
			floatTween.targetFloat = targetRotation;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetRotation);
			floatTween2.AddOnFinishCallback(OnTweenFinished);
			floatTween2.ignoreTimeScale = ignoreTimeScale;
			floatTween2.easing = m_Easing;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	private void SetRotation(float rotation)
	{
		base.transform.eulerAngles = new Vector3(base.transform.rotation.x, rotation, base.transform.rotation.z);
	}

	protected virtual void OnTweenFinished()
	{
		StartTween(360f, m_Duration, ignoreTimeScale: true);
	}
}
