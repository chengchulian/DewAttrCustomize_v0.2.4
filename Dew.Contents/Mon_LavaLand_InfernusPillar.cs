using UnityEngine.AI;

public class Mon_LavaLand_InfernusPillar : Monster
{
	public NavMeshObstacle obstacle;

	public float projectileInterval;

	private float _lastProjectileTime;

	private Mon_LavaLand_BossInfernus _infernus;

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}
}
