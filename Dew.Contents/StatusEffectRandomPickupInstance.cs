public class StatusEffectRandomPickupInstance : RandomPickupInstance
{
	public bool destroyExisting = true;

	public StatusEffect statusEffect;

	protected override void OnPickup(Hero hero)
	{
		base.OnPickup(hero);
		if (destroyExisting)
		{
			StatusEffect statusEffect = hero.Status.FindStatusEffect((StatusEffect se) => se.GetType() == this.statusEffect.GetType());
			if (statusEffect != null)
			{
				statusEffect.Destroy();
			}
		}
		CreateStatusEffect(this.statusEffect, hero, default(CastInfo));
	}

	private void MirrorProcessed()
	{
	}
}
