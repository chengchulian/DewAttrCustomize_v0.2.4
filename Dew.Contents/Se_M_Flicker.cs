using System.Collections;
using UnityEngine;

public class Se_M_Flicker : StatusEffect
{
	public float disappearTime;

	protected override IEnumerator OnCreateSequenced()
	{
		base.info.caster.Visual.DisableRenderersLocal();
		if (base.isServer)
		{
			DoInvulnerable();
			DoUncollidable();
			DoInvisible();
			Vector3 dest = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, base.info.point);
			base.info.caster.Control.StartDaze(disappearTime);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				canGoOverTerrain = true,
				destination = dest,
				duration = disappearTime,
				ease = DewEase.EaseOutQuad,
				isCanceledByCC = false,
				isFriendly = true,
				rotateForward = false
			});
			yield return new SI.WaitForSeconds(disappearTime);
			FxPlayNewNetworked(endEffect, dest, base.info.caster.rotation);
			Destroy();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		base.info.caster.Visual.EnableRenderersLocal();
	}

	private void MirrorProcessed()
	{
	}
}
