public class Se_C_CorrosiveTrails_Spawner : StatusEffect
{
	public float duration;

	public ScalingValue speedAmount;

	public float minDistanceBetweenInstances;

	private float _lastSpawnGasTimer;

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	private void MirrorProcessed()
	{
	}
}
