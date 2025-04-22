using UnityEngine;

public class Mon_Despair_UnstableRat : Monster
{
	public float runDirRefreshTime;

	public Vector2 runDuration;

	public float maxDistance;

	private float _currentRunDuration;

	private float _runStartTime;

	private float _lastRunDirectionTime;

	private Vector3 _dir;

	private float _rotSpeed;

	private bool _isRunning;

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	protected override void OnDeath(EventInfoKill info)
	{
	}

	public void StartRunning()
	{
	}

	public void StopRunning()
	{
	}

	private Vector3 GetRunFromTargetDestination(Entity target)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
