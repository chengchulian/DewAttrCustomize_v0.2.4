public class Ai_Mon_Sky_StarSeed_SelfDestruct_Projectile : StandardProjectile
{
	public ScalingValue damage;

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		Damage(damage).SetElemental(ElementalType.Light).Dispatch(hit.entity);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
