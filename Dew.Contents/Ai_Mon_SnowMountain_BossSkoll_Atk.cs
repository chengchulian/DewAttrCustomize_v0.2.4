using UnityEngine;

public class Ai_Mon_SnowMountain_BossSkoll_Atk : AbilityInstance
{
	public DewAnimationClip animDash;

	public GameObject fxDash;

	public float dashMinDistance;

	public float dashLengthOffset;

	public DewEase dashEase;

	public float dashSpeed;

	public DewAnimationClip animAtkPrepare;

	public GameObject fxAtkPrepareAttached;

	public GameObject fxAtkPrepareTelegraph;

	public float atkPrepareDuration;

	public DewAnimationClip animAtkCast;

	public float swipeChance = 0.25f;

	public float postAttackBeforeSwipeDelay = 0.35f;

	public float postAttackDelay = 1f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			FxPlayNetworked(fxDash, base.info.caster);
			Vector3 vector = base.info.target.agentPosition - base.info.caster.agentPosition;
			vector = vector.normalized * (vector.magnitude + dashLengthOffset);
			if (vector.magnitude < dashMinDistance)
			{
				vector = vector.normalized * dashMinDistance;
			}
			Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, base.info.caster.agentPosition + vector);
			float duration = (base.info.caster.agentPosition - validAgentDestination_LinearSweep).magnitude / dashSpeed;
			base.info.caster.Animation.PlayAbilityAnimation(animDash);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				duration = duration,
				destination = validAgentDestination_LinearSweep,
				ease = dashEase,
				affectedByMovementSpeed = true,
				rotateSmoothly = false,
				isFriendly = true,
				canGoOverTerrain = true,
				rotateForward = true,
				isCanceledByCC = false,
				onCancel = delegate
				{
					FxStopNetworked(fxDash);
					DestroyIfActive();
				},
				onFinish = delegate
				{
					FxStopNetworked(fxDash);
					ChannelAttack();
				}
			});
		}
	}

	private void ChannelAttack()
	{
		float speed = base.info.caster.Status.attackSpeedMultiplier;
		Vector3 pos = base.info.caster.position;
		Quaternion rot = Quaternion.Euler(0f, AbilityTrigger.PredictAngle_Simple(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), base.info.target, pos, atkPrepareDuration / speed), 0f);
		base.info.caster.Control.Rotate(rot, immediately: true);
		FxPlayNetworked(fxAtkPrepareAttached, base.info.caster);
		FxPlayNetworked(fxAtkPrepareTelegraph, pos, rot);
		base.info.caster.Animation.PlayAbilityAnimation(animAtkPrepare, speed);
		base.info.caster.Control.StartChannel(new Channel
		{
			duration = atkPrepareDuration / speed,
			blockedActions = Channel.BlockedAction.Everything,
			onCancel = delegate
			{
				FxStopNetworked(fxAtkPrepareAttached);
				FxStopNetworked(fxAtkPrepareTelegraph);
				DestroyIfActive();
				base.firstTrigger.SetCooldownTime(0, 1f);
			},
			onComplete = delegate
			{
				FxStopNetworked(fxAtkPrepareAttached);
				FxStopNetworked(fxAtkPrepareTelegraph);
				base.info.caster.Animation.PlayAbilityAnimation(animAtkCast, speed);
				CreateAbilityInstance<Ai_Mon_SnowMountain_BossSkoll_Atk_Damage>(pos, rot, new CastInfo(base.info.caster));
				if (Random.value < swipeChance * (0.5f + NetworkedManagerBase<GameManager>.instance.difficulty.specialSkillChanceMultiplier * 0.5f))
				{
					base.info.caster.Control.StartDaze(postAttackBeforeSwipeDelay);
					((Mon_SnowMountain_BossSkoll)base.info.caster).AllowSwipe();
				}
				else
				{
					base.info.caster.Control.StartDaze(postAttackDelay);
				}
				DestroyIfActive();
			}
		}.AddValidation(new AbilitySelfValidator()));
	}

	private void MirrorProcessed()
	{
	}
}
