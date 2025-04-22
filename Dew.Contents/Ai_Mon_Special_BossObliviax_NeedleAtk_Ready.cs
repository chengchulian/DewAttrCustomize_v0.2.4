using System;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_NeedleAtk_Ready : AbilityInstance
{
	public DewAnimationClip castAnimClip;

	public DewAnimationClip endAnimClip;

	public float counterDuration;

	public float takenDamageGraceTime;

	public float startGraceTime;

	public int maxDamageTakeCount;

	public GameObject fxOnStackFull;

	private float _damageTakenTime = -1f;

	private int _takenCount;

	private Channel _channel;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			base.info.caster.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(OnTakeDamage);
			maxDamageTakeCount += (Dew.GetAliveHeroCount() - 1) * 2;
			base.info.caster.Visual.genericStackIndicatorMax = maxDamageTakeCount;
			CreateBasicEffect(base.info.caster, new ArmorBoostEffect
			{
				strength = 200f
			}, 10f, "NeedleAtkShielding").DestroyOnDestroy(this);
			CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 10f, "ObliviaxUnstoppable").DestroyOnDestroy(this);
			_channel = base.info.caster.Control.StartChannel(new Channel
			{
				blockedActions = Channel.BlockedAction.Everything,
				duration = counterDuration,
				onCancel = delegate
				{
					base.info.caster.Animation.PlayAbilityAnimation(endAnimClip);
				},
				onComplete = delegate
				{
					base.info.caster.Visual.genericStackIndicatorValue = 0;
					base.info.caster.Control.StartDaze(1f);
					DestroyIfActive();
				}
			});
			base.info.caster.Animation.PlayAbilityAnimation(castAnimClip);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			base.info.caster.Visual.genericStackIndicatorValue = 0;
			base.info.caster.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(OnTakeDamage);
		}
	}

	private void OnTakeDamage(EventInfoDamage eventInfo)
	{
		if (!(eventInfo.actor is ElementalStatusEffect) && !(Time.time - base.creationTime < startGraceTime) && !(Time.time - _damageTakenTime < takenDamageGraceTime))
		{
			_damageTakenTime = Time.time;
			_takenCount++;
			base.info.caster.Visual.genericStackIndicatorValue = _takenCount;
			if (_takenCount >= maxDamageTakeCount)
			{
				_channel.Cancel();
				CreateAbilityInstance<Ai_Mon_Special_BossObliviax_NeedleAtk>(base.info.caster.position, null, base.info);
				DestroyIfActive();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
