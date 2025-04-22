public class Mon_Forest_Scarab : Monster
{
	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			base.AI.Helper_ChaseTarget();
		}
	}

	private void MirrorProcessed()
	{
	}
}
