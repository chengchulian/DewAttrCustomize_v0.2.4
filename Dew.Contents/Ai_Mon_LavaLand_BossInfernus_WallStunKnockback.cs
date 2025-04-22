using UnityEngine;

public class Ai_Mon_LavaLand_BossInfernus_WallStunKnockback : AbilityInstance
{
	public DewEase ease;

	public DewEase easeOnHitWall;

	public float knockbackMaxDist;

	public float knockbackSpeed;

	public float wallStunDuration;

	public ScalingValue wallStunDmg;

	public GameObject fxWallStun;

	protected override void OnCreate()
	{
	}

	private void MirrorProcessed()
	{
	}
}
