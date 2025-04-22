public class Se_PolarisHallucination : StatusEffect
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoUnstoppable();
			DoDeathInterrupt(delegate
			{
				Destroy();
				base.victim.Status.SetHealth(1f);
				base.victim.Destroy();
			}, -1000);
			base.victim.dealtDamageProcessor.Add(VictimOndealtDamageProcessor);
			base.victim.Visual.deathEffect = null;
			base.victim.Visual.deathBehavior = EntityVisual.EntityDeathBehavior.HideModel;
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.dealtDamageProcessor.Remove(VictimOndealtDamageProcessor);
		}
	}

	private void VictimOndealtDamageProcessor(ref DamageData data, Actor actor, Entity target)
	{
		data.ApplyReduction(0.2f);
	}

	private void MirrorProcessed()
	{
	}
}
