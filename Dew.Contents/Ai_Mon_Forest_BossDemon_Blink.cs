using UnityEngine;

public class Ai_Mon_Forest_BossDemon_Blink : AbilityInstance
{
	public DewEase ease;

	public float duration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, base.info.point);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				destination = validAgentDestination_LinearSweep,
				canGoOverTerrain = false,
				duration = duration,
				ease = ease,
				isCanceledByCC = false,
				isFriendly = true,
				onCancel = base.Destroy,
				onFinish = base.Destroy,
				rotateForward = true
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
