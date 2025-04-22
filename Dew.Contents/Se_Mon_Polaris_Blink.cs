using UnityEngine;

public class Se_Mon_Polaris_Blink : StatusEffect
{
	public DewEase ease = DewEase.EaseOutQuad;

	public float speed = 20f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoUncollidable();
			DoInvulnerable();
			DoInvisible();
			base.victim.Visual.DisableRenderers();
			Vector3 validAgentPosition = Dew.GetValidAgentPosition(Dew.GetPositionOnGround(base.info.point));
			base.victim.Control.StartDisplacement(new DispByDestination
			{
				destination = validAgentPosition,
				affectedByMovementSpeed = false,
				canGoOverTerrain = true,
				duration = Vector3.Distance(validAgentPosition, base.info.caster.agentPosition) / speed,
				ease = ease,
				isFriendly = true,
				rotateForward = false,
				rotateSmoothly = false,
				isCanceledByCC = false,
				onCancel = base.DestroyIfActive,
				onFinish = base.DestroyIfActive
			});
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.Visual.EnableRenderers();
		}
	}

	private void MirrorProcessed()
	{
	}
}
