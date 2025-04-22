using UnityEngine;

public class Ai_Mon_Forest_SpiderWarrior_Jump : AbilityInstance
{
	public GameObject flyEffect;

	public GameObject landEffect;

	public float duration;

	public float minRange = 3f;

	public float afterLandDelay = 0.5f;

	public float jumpStrength = 0.5f;

	protected override void OnCreate()
	{
		base.OnCreate();
		FxPlay(flyEffect, base.info.caster);
		if (base.isServer)
		{
			Vector3 vector = base.info.point;
			if (Vector3.Distance(base.info.caster.position, vector) < minRange)
			{
				vector = base.info.caster.position + (vector - base.info.caster.position).normalized * minRange;
			}
			Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, vector);
			base.info.caster.Visual.KnockUp(jumpStrength, isFriendly: true);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = true,
				canGoOverTerrain = false,
				destination = validAgentDestination_LinearSweep,
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
				rotateForward = true
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
