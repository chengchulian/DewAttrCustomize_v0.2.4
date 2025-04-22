public class Se_Star_L_ElementalResistance : StarEffect
{
	public float[] durationReduction;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientEntityEventOnStatusEffectAdded(EventInfoStatusEffect obj)
	{
	}

	private void ReduceBurnDamage(ref DamageData data, Actor actor, Entity target)
	{
	}

	private void MirrorProcessed()
	{
	}
}
