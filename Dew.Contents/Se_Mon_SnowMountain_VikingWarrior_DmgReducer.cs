using UnityEngine;

public class Se_Mon_SnowMountain_VikingWarrior_DmgReducer : StatusEffect
{
	public float dmgReduction;

	public GameObject fxBlocked;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			CreateBasicEffect(base.victim, new UnstoppableEffect(), float.PositiveInfinity);
			base.victim.takenDamageProcessor.Add(ReduceDamage);
			DestroyOnDeath(base.victim);
		}
	}

	private void ReduceDamage(ref DamageData data, Actor actor, Entity target)
	{
		Entity entity = actor.firstEntity;
		Vector2 lhs = base.victim.transform.forward.ToXY();
		if (!entity.IsNullInactiveDeadOrKnockedOut() && entity is Hero && (!(Vector2.Dot(lhs, (entity.position - base.victim.position).ToXY()) <= 0f) || (data.direction.HasValue && !(Vector2.Dot(lhs, (-data.direction.Value).ToXY()) <= 0f)) || (data.originPosition.HasValue && !(Vector2.Dot(lhs, (data.originPosition.Value - base.victim.position).ToXY()) <= 0f))) && base.victim.Control.ongoingChannels.Count <= 0 && !(actor is ElementalStatusEffect))
		{
			data.ApplyReduction(dmgReduction);
			FxPlayNewNetworked(fxBlocked, base.victim);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			base.victim.takenDamageProcessor.Remove(ReduceDamage);
		}
	}

	private void MirrorProcessed()
	{
	}
}
