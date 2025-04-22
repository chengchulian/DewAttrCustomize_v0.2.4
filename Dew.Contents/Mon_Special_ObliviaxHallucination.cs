public class Mon_Special_ObliviaxHallucination : Monster
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity);
			CreateBasicEffect(this, new InvisibleEffect(), float.PositiveInfinity);
			CreateBasicEffect(this, new InvulnerableEffect(), float.PositiveInfinity);
			CreateBasicEffect(this, new UncollidableEffect(), float.PositiveInfinity);
		}
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		_ = context.targetEnemy == null;
	}

	private void MirrorProcessed()
	{
	}
}
