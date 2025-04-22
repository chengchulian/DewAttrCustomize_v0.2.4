public class Ai_Mon_Forest_BossDemon_MainSkill_Stomp_Tree : InstantDamageInstance
{
	public Knockback knockback;

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		knockback.ApplyWithOrigin(base.transform.position, entity);
	}

	private void MirrorProcessed()
	{
	}
}
