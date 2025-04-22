using UnityEngine;

public class Mon_LavaLand_BossInfernus : BossMonster
{
	public float widenShoulderAmount;

	public float headLiftAngle;

	public float invulShieldThreshold;

	public float breathFireThreshold;

	private Transform _spine;

	private Transform _leftShoulder;

	private Transform _rightShoulder;

	private Transform _head;

	public override void OnStartServer()
	{
	}

	protected override void OnCreate()
	{
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	private void LateUpdate()
	{
	}

	protected override void OnBossSoulBeforeSpawn(Shrine_BossSoul soul)
	{
	}

	private void MirrorProcessed()
	{
	}
}
