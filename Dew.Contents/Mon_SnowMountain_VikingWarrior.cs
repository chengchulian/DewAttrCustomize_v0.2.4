using DG.Tweening;
using UnityEngine;

public class Mon_SnowMountain_VikingWarrior : Monster
{
	public Transform shieldTransform;

	public Transform helmetTransform;

	public override void OnStartServer()
	{
		base.OnStartServer();
		CreateStatusEffect<Se_Mon_SnowMountain_VikingWarrior_DmgReducer>(this, new CastInfo(this));
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_VikingWarrior_SpawnSword>() && base.AI.Helper_CanBeCast<At_Mon_SnowMountain_VikingWarrior_SpawnSword>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_VikingWarrior_SpawnSword>();
			}
			else
			{
				base.AI.Helper_ChaseTarget();
			}
		}
	}

	protected override void OnDeath(EventInfoKill info)
	{
		base.OnDeath(info);
		shieldTransform.DOKill();
		shieldTransform.localScale = Vector3.zero;
		helmetTransform.DOKill();
		helmetTransform.localScale = Vector3.zero;
	}

	public void HideWeaponTemporarilyLocal()
	{
		shieldTransform.DOKill();
		DOTween.Sequence().Append(shieldTransform.DOScale(Vector3.zero, 0.1f)).AppendInterval(0.75f)
			.Append(shieldTransform.DOScale(Vector3.one, 0.3f))
			.SetId(shieldTransform);
		helmetTransform.DOKill();
		DOTween.Sequence().Append(helmetTransform.DOScale(Vector3.zero, 0.1f)).AppendInterval(0.75f)
			.Append(helmetTransform.DOScale(Vector3.one, 0.3f))
			.SetId(helmetTransform);
	}

	private void MirrorProcessed()
	{
	}
}
