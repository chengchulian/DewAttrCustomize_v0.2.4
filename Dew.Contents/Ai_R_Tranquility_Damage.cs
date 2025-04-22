public class Ai_R_Tranquility_Damage : TickDamageInstance
{
	public float stunDuration = 1f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DestroyOnDestroy(base.parentActor);
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = base.info.caster.agentPosition;
	}

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		CreateBasicEffect(entity, new StunEffect(), stunDuration, "TranquilityStun", DuplicateEffectBehavior.UsePrevious);
	}

	private void MirrorProcessed()
	{
	}
}
