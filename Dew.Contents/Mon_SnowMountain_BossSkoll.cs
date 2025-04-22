using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

public class Mon_SnowMountain_BossSkoll : BossMonster
{
	public float auraBladeChance;

	public float summonSwordChance;

	public float summonSwordMinDist;

	public float deathFromAboveChance;

	public float deathFromAboveHpRatio;

	public float whirlwindHpRatio;

	public float allowSwipeTimeout = 1f;

	public float dropGlacialCoreChance = 0.1f;

	internal float _allowSwipeTime = float.NegativeInfinity;

	public DewAnimationClip cutSceneAnimationClip;

	public float cutSceneAnimationDelay;

	public GameObject cutSceneEffect;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity);
		}
	}

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (base.isServer)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			float seconds = base.Visual.spawnDuration + cutSceneAnimationDelay;
			yield return new WaitForSeconds(seconds);
			if (ManagerBase<CameraManager>.instance.isPlayingCutscene)
			{
				FxPlayNetworked(cutSceneEffect, this);
				base.Animation.PlayAbilityAnimation(cutSceneAnimationClip);
				float rawClipDuration = base.Animation.abilityAnimStatus.rawClipDuration;
				base.Control.StartDaze(rawClipDuration);
			}
		}
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (Time.time - _allowSwipeTime < allowSwipeTimeout && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_BossSkoll_Swipe>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_BossSkoll_Swipe>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_BossSkoll_Swipe>();
				_allowSwipeTime = float.NegativeInfinity;
			}
			if (Random.value < auraBladeChance * context.deltaTime && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_BossSkoll_AuraBlade>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_BossSkoll_AuraBlade>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_BossSkoll_AuraBlade>();
			}
			else if (Random.value < summonSwordChance * context.deltaTime && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_BossSkoll_SummonSword>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_BossSkoll_SummonSword>() && Vector2.Distance(context.targetEnemy.agentPosition.ToXY(), base.agentPosition.ToXY()) > summonSwordMinDist)
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_BossSkoll_SummonSword>();
			}
			else if (Random.value < deathFromAboveChance * context.deltaTime && base.normalizedHealth < deathFromAboveHpRatio && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_BossSkoll_DeathFromAbove>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_BossSkoll_DeathFromAbove>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_BossSkoll_DeathFromAbove>();
			}
			else if (base.normalizedHealth < whirlwindHpRatio && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_BossSkoll_Whirlwind>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_BossSkoll_Whirlwind>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_BossSkoll_Whirlwind>();
			}
			else
			{
				base.AI.Helper_ChaseTarget();
			}
		}
	}

	[Server]
	public void AllowSwipe()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void Mon_SnowMountain_BossSkoll::AllowSwipe()' called when server was not active");
		}
		else
		{
			_allowSwipeTime = Time.time;
		}
	}

	protected override void OnBossSoulBeforeSpawn(Shrine_BossSoul soul)
	{
		base.OnBossSoulBeforeSpawn(soul);
		
		if (!AttrCustomizeResources.Config.removeGems.Contains("Gem_L_GlacialCore"))
		{
			soul.SetGemReward<Gem_L_GlacialCore>(dropGlacialCoreChance);
		}
	}

	private void MirrorProcessed()
	{
	}
}
