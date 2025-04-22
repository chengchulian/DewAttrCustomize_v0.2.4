public class Se_Mon_SnowMountain_BossSkoll_DeathFromAbove_Invulnerable : StatusEffect
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoInvulnerable();
			DoInvisible();
			DoUncollidable();
			base.info.caster.Control.freeMovement = true;
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.info.caster == null))
		{
			base.info.caster.Control.freeMovement = false;
		}
	}

	private void MirrorProcessed()
	{
	}
}
