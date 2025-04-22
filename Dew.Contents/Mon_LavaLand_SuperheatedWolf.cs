using UnityEngine;

public class Mon_LavaLand_SuperheatedWolf : Monster
{
	public float selfDestructChance;

	public float selfDestructHpRatioThreshold;

	public float selfDestructPropagateRadius;

	public Vector2 selfDestructPropagateDelay;

	private bool _wantsToSelfDestructImmediately;

	private bool _didPropagate;

	public override void OnStartServer()
	{
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	private void MirrorProcessed()
	{
	}
}
