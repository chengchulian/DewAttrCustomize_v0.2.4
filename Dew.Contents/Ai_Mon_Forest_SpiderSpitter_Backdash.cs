using UnityEngine;

public class Ai_Mon_Forest_SpiderSpitter_Backdash : AbilityInstance
{
	public GameObject flyEffect;

	public GameObject landEffect;

	public float duration = 0.5f;

	public float distance = 3f;

	public float minDistance = 2f;

	public float afterLandDelay = 0.5f;

	public float jumpStrength;

	protected override void OnCreate()
	{
		base.OnCreate();
		FxPlay(flyEffect, base.info.caster);
		if (!base.isServer)
		{
			return;
		}
		Vector3 vector = Vector3.zero;
		float num = float.NegativeInfinity;
		for (float num2 = 0f; num2 <= 360f; num2 += 15f)
		{
			Vector3 end = base.info.caster.agentPosition + Quaternion.Euler(0f, num2, 0f) * Vector3.forward * distance;
			end = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, end);
			float num3 = Vector3.Distance(base.info.target.agentPosition, end);
			if (num3 > num)
			{
				num = num3;
				vector = end;
			}
		}
		base.info.caster.Control.Rotate(base.info.caster.position - vector, immediately: false);
		base.info.caster.Visual.KnockUp(jumpStrength, isFriendly: true);
		base.info.caster.Control.StartDisplacement(new DispByDestination
		{
			affectedByMovementSpeed = true,
			canGoOverTerrain = false,
			destination = vector,
			duration = duration,
			ease = DewEase.Linear,
			isCanceledByCC = true,
			isFriendly = true,
			onCancel = base.Destroy,
			onFinish = delegate
			{
				base.info.caster.Control.StartDaze(afterLandDelay);
				FxPlayNetworked(landEffect, base.info.caster);
				Destroy();
			},
			rotateForward = false
		});
	}

	private void MirrorProcessed()
	{
	}
}
