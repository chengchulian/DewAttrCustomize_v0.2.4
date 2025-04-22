using UnityEngine;

public class Ai_StarCookie_Projectile : StandardProjectile
{
	public float startHeight;

	public Vector2 duration;

	protected override void OnPrepare()
	{
		base.OnPrepare();
		base.targetPosition = base.transform.position + Vector3.up * customEndHeight;
		float num = customEndHeight;
		SetCustomStartPosition(base.transform.position + Vector3.up * startHeight);
		base.initialSpeed = (startHeight - num) / Random.Range(duration.x, duration.y);
		_acceleration = 0f;
	}

	protected override void OnComplete()
	{
		base.OnComplete();
		if (base.isServer)
		{
			Dew.CreateActor<Shrine_StarCookie>(base.targetPosition, null);
		}
	}

	private void MirrorProcessed()
	{
	}
}
