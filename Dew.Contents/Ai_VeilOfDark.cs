using UnityEngine;

public class Ai_VeilOfDark : AbilityInstance
{
	public float radius;

	public float collisionCheckInterval;

	public float duration;

	public GameObject fxInstance;

	public GameObject range;

	private float _currentTime;

	private float _elapsedTime;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	protected override void ActiveFrameUpdate()
	{
	}

	private void MirrorProcessed()
	{
	}
}
