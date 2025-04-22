public class RoomMod_RiskOfMeteors : RoomModifierBase
{
	public float startDelay;

	public float targetedMeteorChancePerPlayer;

	public float waveInterval;

	public float wavePerShotInterval;

	public float meteorCountInWavePerArea;

	public int maxMeteorCountInWave;

	public float spreadOutWaveDuration;

	private int _meteorCount;

	private float _nextWaveTime;

	private float _nextRegularTime;

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	private void SpawnMeteor(float targetedMeteorChance)
	{
	}

	private void MirrorProcessed()
	{
	}
}
