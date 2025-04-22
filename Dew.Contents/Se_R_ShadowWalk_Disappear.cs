public class Se_R_ShadowWalk_Disappear : StatusEffect
{
	public float disappearDuration = 0.25f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.info.caster.Visual.DisableRenderers();
			DoUncollidable();
			DoInvulnerable();
			SetTimer(disappearDuration);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			base.info.caster.Visual.EnableRenderers();
		}
	}

	private void MirrorProcessed()
	{
	}
}
