using System;
using FIMSpace.FTail;
using UnityEngine;

public class Hero_Lacerta : Hero
{
	public TailAnimator2 tailAnimator;

	[NonSerialized]
	public bool actDeadInTutorial;

	private float _defaultWavingSpeed;

	protected override void Awake()
	{
		base.Awake();
		_defaultWavingSpeed = tailAnimator.WavingSpeed;
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		tailAnimator.WavingSpeed = ((base.isKnockedOut || actDeadInTutorial) ? 0f : _defaultWavingSpeed);
		tailAnimator.Gravity = Vector3.MoveTowards(tailAnimator.Gravity, (base.isKnockedOut || actDeadInTutorial) ? new Vector3(0f, -10f, 0f) : Vector3.zero, dt * 5f);
	}

	private void MirrorProcessed()
	{
	}
}
