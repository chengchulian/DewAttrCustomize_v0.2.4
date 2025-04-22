public class Se_Curse_FragmentedBeing : CurseStatusEffect
{
	public float[] ampAmounts;

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
			if (!(entity == null) && entity.GetRelation(target) == EntityRelation.Enemy)
			{
				data.ApplyAmplification(GetValue(ampAmounts));
				data.SetAmountModifiedBy(this);
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
