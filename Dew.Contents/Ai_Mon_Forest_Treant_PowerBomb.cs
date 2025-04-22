public class Ai_Mon_Forest_Treant_PowerBomb : InstantDamageInstance
{
	public Knockback knockback;

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		knockback.ApplyWithOrigin(base.info.caster.position, entity);
	}

	private void MirrorProcessed()
	{
	}
}
