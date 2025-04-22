using System;
using System.Collections;
using UnityEngine;

public class Shrine_Guidance : Shrine
{
	public DewCollider range;

	public float healRatio;

	public int ticks;

	public float tickInterval;

	public float healDelayByDistance;

	public float explodeDelay;

	public GameObject healExplodeEffect;

	public Action<Entity> actionOverride;

	protected override bool OnUse(Entity entity)
	{
		StartCoroutine(Routine());
		return true;
		IEnumerator HealRoutine(Entity e, float delay)
		{
			yield return new WaitForSeconds(delay);
			if (actionOverride == null)
			{
				CreateStatusEffect(e, default(CastInfo), delegate(Se_GenericHealOverTime h)
				{
					h.ticks = ticks;
					h.tickInterval = tickInterval;
					h.totalAmount = healRatio * e.maxHealth;
				});
			}
			else
			{
				actionOverride?.Invoke(e);
			}
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(explodeDelay);
			FxPlayNewNetworked(healExplodeEffect);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, new CollisionCheckSettings
			{
				includeUncollidable = true
			});
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity2 = entities[i];
				if (entity2.owner.isHumanPlayer)
				{
					StartCoroutine(HealRoutine(entity2, Vector3.Distance(base.position, entity2.position) * healDelayByDistance));
				}
			}
			handle.Return();
		}
	}

	public override Cost GetCost(Entity activator)
	{
		Cost cost = base.GetCost(activator);
		if (!activator.Status.TryGetStatusEffect<Se_Star_L_GuidanceShrineDiscount>(out var effect))
		{
			return cost;
		}
		return (Cost)(cost * (1f - effect.GetValue(effect.discountRatio)));
	}

	private void MirrorProcessed()
	{
	}
}
