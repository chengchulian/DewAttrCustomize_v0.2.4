using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Se_Mon_Sky_BossNyx_PhaseChange : StatusEffect
{
	public bool doInvulnerable;

	public bool killOtherMonsters;

	[Space(15f)]
	public float staggerDuration;

	public DewAnimationClip staggerClip;

	public GameObject staggerEffect;

	[Space(15f)]
	public float prepareExplodeDuration;

	public DewAnimationClip prepareExplodeClip;

	public GameObject prepareExplodeEffect;

	public GameObject explodeTelegraphEffect;

	public bool displaceToCenter;

	public float displaceDuration;

	public DewEase displaceEase;

	public Knockback explodeKnockback;

	[Space(15f)]
	public DewAnimationClip explodeClip;

	public GameObject[] explodeEffects;

	public DewCollider explodeRange;

	public ScalingValue explodeDamage;

	public GameObject explodeHitEffect;

	[Space(15f)]
	public float afterExplodeDuration;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		if (killOtherMonsters)
		{
			foreach (Entity item in new List<Entity>(NetworkedManagerBase<ActorManager>.instance.allEntities))
			{
				if (item.isActive && !item.Status.isDead && item is Monster { type: not Monster.MonsterType.Boss })
				{
					item.Kill();
				}
			}
		}
		if (doInvulnerable)
		{
			DoInvulnerable();
		}
		base.info.caster.Control.Stop();
		base.info.caster.Control.CancelOngoingChannels();
		base.info.caster.Control.StartDaze(staggerDuration + prepareExplodeDuration + afterExplodeDuration);
		base.info.caster.Animation.PlayAbilityAnimation(staggerClip);
		FxPlayNetworked(staggerEffect, base.victim);
		yield return new SI.WaitForSeconds(staggerDuration);
		FxStopNetworked(staggerEffect);
		Vector3 agentPosition = base.info.caster.agentPosition;
		if (SingletonBehaviour<Sky_BossRoomCenter>.instance != null && displaceToCenter)
		{
			agentPosition = SingletonBehaviour<Sky_BossRoomCenter>.instance.transform.position;
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = false,
				canGoOverTerrain = true,
				destination = agentPosition,
				duration = displaceDuration,
				ease = displaceEase,
				isCanceledByCC = false,
				isFriendly = true,
				rotateForward = false
			});
		}
		agentPosition = Dew.GetPositionOnGround(agentPosition);
		base.info.caster.Animation.PlayAbilityAnimation(prepareExplodeClip);
		FxPlayNetworked(prepareExplodeEffect, base.victim);
		FxPlayNewNetworked(explodeTelegraphEffect, agentPosition, Quaternion.identity);
		base.info.caster.Control.Rotate(Vector3.back, immediately: false, prepareExplodeDuration + afterExplodeDuration);
		yield return new SI.WaitForSeconds(prepareExplodeDuration);
		FxStopNetworked(prepareExplodeEffect);
		base.info.caster.Animation.PlayAbilityAnimation(explodeClip);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = explodeRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			Damage(explodeDamage).SetOriginPosition(base.info.caster.position).Dispatch(entity);
			FxPlayNewNetworked(explodeHitEffect, entity);
			explodeKnockback.ApplyWithOrigin(base.info.caster.position, entity);
		}
		handle.Return();
		if (base.victim is Mon_Sky_BossNyx mon_Sky_BossNyx)
		{
			if (mon_Sky_BossNyx._currentPhase >= 0 && mon_Sky_BossNyx._currentPhase < explodeEffects.Length)
			{
				FxPlayNetworked(explodeEffects[mon_Sky_BossNyx._currentPhase], base.victim);
			}
			else
			{
				FxPlayNetworked(explodeEffects[0], base.victim);
			}
			mon_Sky_BossNyx.Network_currentPhase = mon_Sky_BossNyx._currentPhase + 1;
		}
		else
		{
			FxPlayNetworked(explodeEffects[0], base.victim);
		}
		GiveShield(base.victim, base.victim.Status.maxHealth * 0.2f, 6f, isDecay: true);
		if (base.info.caster.Ability.TryGetAbility<At_Mon_Sky_BossNyx_StellarDash>(out var trigger))
		{
			trigger.ResetCooldown();
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
