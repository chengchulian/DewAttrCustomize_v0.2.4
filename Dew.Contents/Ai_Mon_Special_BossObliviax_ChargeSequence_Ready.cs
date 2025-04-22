using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_ChargeSequence_Ready : AbilityInstance
{
	public float chargeDuration;

	public float postDelay;

	public float shieldRatio;

	public GameObject fxComplete;

	public GameObject fxCancel;

	public DewAnimationClip castingAnimClip;

	public DewAnimationClip completeAnimClip;

	public DewAnimationClip endAnimClip;

	private float _damageTakenTime = -1f;

	private bool _isChargeComplete;

	private float _time;

	private int _takenCount;

	private bool _isShieldBroken;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			ShieldEffect shield = GiveShield(base.info.caster, base.info.caster.Status.maxHealth * shieldRatio, chargeDuration).shield;
			shield.onDamageNegated = (Action<EventInfoDamageNegatedByShield>)Delegate.Combine(shield.onDamageNegated, new Action<EventInfoDamageNegatedByShield>(OnDamageNegatedByShield));
			base.info.caster.Animation.PlayAbilityAnimation(castingAnimClip);
			base.info.caster.Control.StartChannel(new Channel
			{
				blockedActions = Channel.BlockedAction.Everything,
				duration = chargeDuration,
				onCancel = delegate
				{
					_isChargeComplete = true;
					base.info.caster.Animation.PlayAbilityAnimation(endAnimClip);
					_isShieldBroken = true;
					base.info.caster.Control.StartDaze(postDelay);
					FxStopNetworked(startEffect);
					FxPlayNetworked(fxCancel, base.info.caster);
				},
				onComplete = delegate
				{
					_isChargeComplete = true;
					base.info.caster.Animation.PlayAbilityAnimation(completeAnimClip);
					FxStopNetworked(startEffect);
					FxPlayNetworked(fxComplete, base.info.caster);
					CreateAbilityInstance<Ai_Mon_Special_BossObliviax_ChargeSequence_Spawner>(base.info.caster.position, null, new CastInfo(base.info.caster));
				}
			});
			yield return new SI.WaitForCondition(() => _isChargeComplete);
			yield return new SI.WaitForSeconds(postDelay);
			Destroy();
		}
	}

	private void OnDamageNegatedByShield(EventInfoDamageNegatedByShield obj)
	{
		if (obj.shield.amount < 0.001f)
		{
			base.info.caster.Control.CancelOngoingChannels();
			DestroyIfActive();
		}
	}

	private void MirrorProcessed()
	{
	}
}
