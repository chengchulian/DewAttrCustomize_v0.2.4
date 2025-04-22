using System;
using System.Linq;
using UnityEngine;

public class Mon_Forest_SpiderWarrior : Monster, ISpawnableAsMiniBoss
{
	public float jumpRandomPositionMag = 3f;

	public float jumpBehindOfTargetDistance = 2f;

	public float jumpChance = 0.25f;

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (global::UnityEngine.Random.value < jumpChance && !base.AI.Helper_IsTargetInRangeOfAttack() && base.AI.Helper_CanBeCast<At_Mon_Forest_SpiderWarrior_Jump>())
			{
				Vector3 vector = context.targetEnemy.position - base.position;
				Vector3 point = base.position + vector.normalized * (vector.magnitude + jumpBehindOfTargetDistance) + global::UnityEngine.Random.insideUnitCircle.ToXZ() * jumpRandomPositionMag;
				base.AI.Helper_CastAbility<At_Mon_Forest_SpiderWarrior_Jump>(new CastInfo(this, point));
			}
			else
			{
				base.AI.Helper_ChaseTarget();
			}
		}
	}

	public void OnBeforeSpawnAsMiniBoss()
	{
	}

	public void OnCreateAsMiniBoss()
	{
		if (!base.isServer)
		{
			return;
		}
		ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
		RefValue<float> last = new RefValue<float>(1f);
		float[] spawnThresholds = new float[3] { 0.25f, 0.5f, 0.75f };
		EntityEvent_OnTakeDamage += (Action<EventInfoDamage>)delegate
		{
			float num = base.normalizedHealth;
			if (!((float)last < num))
			{
				float[] array = spawnThresholds;
				foreach (float num2 in array)
				{
					if (num < num2 && (float)last >= num2)
					{
						CreateAbilityInstance<Ai_Mon_Forest_SpiderWarrior_SpawnScarabs>(base.position, Quaternion.identity, new CastInfo(this));
					}
				}
				last.value = num;
			}
		};
	}

	protected override void OnDeath(EventInfoKill info)
	{
		base.OnDeath(info);
		if (!base.isServer)
		{
			return;
		}
		Actor[] array = base.children.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is Mon_Forest_Scarab mon_Forest_Scarab && !mon_Forest_Scarab.IsNullInactiveDeadOrKnockedOut())
			{
				mon_Forest_Scarab.Kill();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
