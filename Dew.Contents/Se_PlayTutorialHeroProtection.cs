public class Se_PlayTutorialHeroProtection : StatusEffect
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoDeathInterrupt(delegate(EventInfoKill k)
			{
				k.victim.Status.SetHealth(1f);
			}, 0);
			base.victim.takenDamageProcessor.Add(DamageTakenProcessor);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			base.victim.takenDamageProcessor.Remove(DamageTakenProcessor);
		}
	}

	private void DamageTakenProcessor(ref DamageData data, Actor actor, Entity target)
	{
		if (data.source != DamageData.SourceType.Pure && base.victim.normalizedHealth < 0.5f)
		{
			data.ApplyRawMultiplier(base.victim.normalizedHealth);
		}
	}

	private void MirrorProcessed()
	{
	}
}
