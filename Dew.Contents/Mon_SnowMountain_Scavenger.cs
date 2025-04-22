using DG.Tweening;
using UnityEngine;

public class Mon_SnowMountain_Scavenger : Monster, ISpawnableAsMiniBoss
{
	public float throwChance = 0.5f;

	public float jumpRandomPositionMag = 3f;

	public float jumpBehindOfTargetDistance = 2f;

	public float jumpChance = 0.25f;

	public Transform weaponTransform;

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (Random.value < throwChance && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_Scavenger_Throw>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_Scavenger_Throw>() && !base.AI.Helper_IsTargetInRangeOfAttack())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_Scavenger_Throw>();
			}
			else if (Random.value < jumpChance && !base.AI.Helper_IsTargetInRangeOfAttack() && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_Scavenger_Jump>())
			{
				Vector3 vector = context.targetEnemy.position - base.position;
				Vector3 point = base.position + vector.normalized * (vector.magnitude + jumpBehindOfTargetDistance) + Random.insideUnitCircle.ToXZ() * jumpRandomPositionMag;
				base.AI.Helper_CastAbility<At_Mon_SnowMountain_Scavenger_Jump>(new CastInfo(this, point));
			}
			else
			{
				base.AI.Helper_ChaseTarget();
			}
		}
	}

	public void HideWeaponTemporarilyLocal()
	{
		weaponTransform.DOKill();
		DOTween.Sequence().Append(weaponTransform.DOScale(Vector3.zero, 0.1f)).AppendInterval(0.75f)
			.Append(weaponTransform.DOScale(Vector3.one, 0.3f))
			.SetId(weaponTransform);
	}

	protected override void OnDeath(EventInfoKill info)
	{
		base.OnDeath(info);
		weaponTransform.DOKill();
		weaponTransform.localScale = Vector3.zero;
	}

	public void OnBeforeSpawnAsMiniBoss()
	{
	}

	public void OnCreateAsMiniBoss()
	{
		if (base.isServer)
		{
			ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
			base.Ability.GetAbility<At_Mon_SnowMountain_Scavenger_Jump>().configs[0].cooldownTime = 0.65f;
			jumpChance = 1f;
		}
	}

	private void MirrorProcessed()
	{
	}
}
