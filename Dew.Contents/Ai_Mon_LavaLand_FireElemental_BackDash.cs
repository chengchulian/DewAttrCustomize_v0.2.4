using UnityEngine;

public class Ai_Mon_LavaLand_FireElemental_BackDash : AbilityInstance
{
	public float dashDuration;

	public float dashDistance;

	public float finishDelay;

	public GameObject dashEffect;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), dashDuration);
		FxPlayNetworked(dashEffect, base.info.caster);
		Vector3 vector = base.info.caster.position + (base.info.caster.position - base.info.target.position).normalized * dashDistance;
		base.info.caster.Control.Rotate(base.info.caster.position - vector, immediately: false, dashDuration);
		base.info.caster.Control.StartDisplacement(new DispByDestination
		{
			affectedByMovementSpeed = true,
			destination = vector,
			duration = dashDuration,
			ease = DewEase.EaseOutQuad,
			isFriendly = true,
			onFinish = delegate
			{
				if (base.isActive)
				{
					base.info.caster.Control.StartDaze(finishDelay);
					Destroy();
				}
			},
			onCancel = base.Destroy,
			rotateForward = false,
			canGoOverTerrain = false,
			isCanceledByCC = false
		});
	}

	private void MirrorProcessed()
	{
	}
}
