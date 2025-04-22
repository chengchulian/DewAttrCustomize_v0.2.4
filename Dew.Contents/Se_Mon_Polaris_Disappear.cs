public class Se_Mon_Polaris_Disappear : StatusEffect
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoUncollidable();
			DoInvulnerable();
			DoInvisible();
			base.victim.Visual.DisableRenderers();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.Visual.EnableRenderers();
		}
	}

	private void MirrorProcessed()
	{
	}
}
