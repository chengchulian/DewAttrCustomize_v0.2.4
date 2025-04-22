using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Sky_LittleBaam : Monster
{
	[HideInInspector]
	public bool isSummoned;

	public int summonCount = 2;

	public float summonMinDis = 1f;

	public float summonDelay = 0.3f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer && !isSummoned)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(summonDelay);
			List<Vector3> spawnedPos = new List<Vector3> { base.transform.position };
			int num = 0;
			int loopCount = 0;
			for (int i = 0; i < summonCount; i++)
			{
				bool flag = true;
				loopCount++;
				Vector3 vector = Random.insideUnitCircle.ToXZ() * 1.5f;
				Vector3 vector2 = spawnedPos[num] + vector;
				vector2 = Dew.GetValidAgentDestination_Closest(vector2, vector2 + vector);
				for (int j = 0; j < spawnedPos.Count; j++)
				{
					if (loopCount > 5)
					{
						loopCount = 0;
						break;
					}
					Vector3 b = spawnedPos[j];
					if (Vector3.Distance(vector2, b) < summonMinDis)
					{
						i--;
						flag = false;
						break;
					}
				}
				if (flag)
				{
					spawnedPos.Add(vector2);
					num++;
					Hero hero = Dew.SelectRandomAliveHero();
					Dew.SpawnEntity(vector2, Quaternion.LookRotation(hero.position - vector2), null, new CastInfo(this).caster.owner, NetworkedManagerBase<GameManager>.instance.ambientLevel, delegate(Mon_Sky_LittleBaam s)
					{
						s.populationCost = 0f;
						s.isSummoned = true;
					});
					yield return new WaitForSeconds(summonDelay);
				}
			}
		}
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (context.targetEnemy == null)
		{
			Hero hero = Dew.SelectRandomAliveHero();
			if (hero != null && !hero.Status.hasInvisible && hero.GetRelation(this) == EntityRelation.Enemy)
			{
				base.AI.Aggro(hero);
			}
		}
		else if (!base.AI.Helper_IsTargetInRangeOfAttack() && base.AI.Helper_CanBeCast<At_Mon_Sky_LittleBaam_Reposition>())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_LittleBaam_Reposition>();
		}
		else
		{
			base.AI.Helper_ChaseTarget();
		}
	}

	private void MirrorProcessed()
	{
	}
}
