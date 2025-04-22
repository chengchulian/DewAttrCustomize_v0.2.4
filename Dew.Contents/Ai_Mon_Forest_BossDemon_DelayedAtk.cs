using System.Collections;
using UnityEngine;

public class Ai_Mon_Forest_BossDemon_DelayedAtk : AbilityInstance
{
	public ChannelData channel;

	public float animBaseDuration = 0.8f;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	public GameObject chargeEffect;

	public GameObject fxTelegraphOnTarget;

	public float postDelay = 0.85f;

	public DewCollider delayedAtkRange;

	public float delayedAtkDashDis = 5f;

	private float _animSelectValue;

	protected override IEnumerator OnCreateSequenced()
	{
		yield return base.OnCreateSequenced();
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		FxPlayNetworked(fxTelegraphOnTarget, base.info.target);
		base.info.caster.Control.RotateTowards(base.info.target, immediately: false);
		for (int i = 0; i < startAnim.entries.Length; i++)
		{
			startAnim.entries[i].duration = animBaseDuration + channel.duration;
		}
		_animSelectValue = Random.value;
		base.info.caster.Animation.PlayAbilityAnimation(startAnim, 1f, _animSelectValue);
		FxPlayNetworked(chargeEffect, base.info.caster);
		channel.Get().AddOnComplete(delegate
		{
			base.info.caster.Control.RotateTowards(base.info.target, immediately: true);
			base.info.caster.Animation.PlayAbilityAnimation(endAnim, 1f, _animSelectValue);
			CastInfo castInfo = new CastInfo(base.info.caster)
			{
				angle = CastInfo.GetAngle(base.info.target.position - base.info.caster.position)
			};
			CreateAbilityInstance(base.info.caster.position, castInfo.rotation, castInfo, delegate(Ai_Mon_Forest_BossDemon_Atk p)
			{
				p.range.points = delayedAtkRange.points;
				p.dash.distance = delayedAtkDashDis;
			});
			base.info.caster.Control.StartDaze(postDelay);
			Destroy();
		}).AddOnCancel(delegate
		{
			base.info.caster.Animation.StopAbilityAnimation(startAnim);
			DestroyIfActive();
		})
			.Dispatch(base.info.caster);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(chargeEffect);
			FxStopNetworked(fxTelegraphOnTarget);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			base.info.caster.Control.RotateTowards(base.info.target, immediately: false);
		}
	}

	private void MirrorProcessed()
	{
	}
}
