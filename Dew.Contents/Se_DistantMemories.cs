public class Se_DistantMemories : StatusEffect
{
	public float dmgAmpAmount;

	public float interval;

	public float movementSpeedReduction;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoSlow(movementSpeedReduction);
			DoInvisible();
			DoProtected(null);
			DoUncollidable();
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
		if (!data.IsAmountModifiedBy(this) && !actor.firstEntity.IsNullInactiveDeadOrKnockedOut())
		{
			data.ApplyAmplification(dmgAmpAmount);
			data.SetAmountModifiedBy(this);
		}
	}

	private void MirrorProcessed()
	{
	}
}
