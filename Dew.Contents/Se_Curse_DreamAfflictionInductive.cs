using UnityEngine;

public class Se_Curse_DreamAfflictionInductive : CurseStatusEffect
{
	public float[] damageHpRatio;

	public Vector2[] interval;

	private float _nextActivateTime;

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
