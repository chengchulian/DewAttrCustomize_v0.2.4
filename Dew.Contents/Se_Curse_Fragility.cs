public class Se_Curse_Fragility : CurseStatusEffect
{
	public float[] instaDeathHealthThresholds;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.takenDamageProcessor.Add(AmplifyDamage);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.takenDamageProcessor.Remove(AmplifyDamage);
		}
	}

	private void AmplifyDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this))
		{
			Entity entity = actor.firstEntity;
			if (!(entity == null) && entity.GetRelation(target) == EntityRelation.Enemy && target.normalizedHealth < GetValue(instaDeathHealthThresholds))
			{
				data.AddFlatAmount(9999999f - data.originalAmount);
				data.SetAmountModifiedBy(this);
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
