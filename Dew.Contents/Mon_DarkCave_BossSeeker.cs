using Mirror;

public class Mon_DarkCave_BossSeeker : BossMonster
{
	public float healthReducedBeforeBlink;

	public float blinkBarrageHealthRatio;

	public float chaserHealthThreshold;

	public float hallucinationHealthThreshold;

	public float fissureHealthThreshold;

	public float tunnelVisionHealthThreshold;

	private float _lastBlinkNormalizedHealth;

	protected override void OnCreate()
	{
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	[Server]
	public void UpdateBlinkHealth()
	{
	}

	private void MirrorProcessed()
	{
	}
}
