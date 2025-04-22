using UnityEngine;

public class Ai_Mon_Despair_Displacer_Blink : AbilityInstance
{
	public float minDistance;

	public Vector2 distanceFromTargetRange;

	public float speed;

	public bool doInvulnerable;

	public bool doUncollidable;

	public float postDelay;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}
}
