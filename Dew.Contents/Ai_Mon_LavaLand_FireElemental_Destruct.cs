using UnityEngine;

public class Ai_Mon_LavaLand_FireElemental_Destruct : AbilityInstance
{
	public GameObject tpStartEffect;

	public GameObject tpEndEffect;

	public float backPosDis;

	public float finishDelay = 2f;

	public float appearDuration = 1f;

	public float shieldMissingHpRatio = 0.4f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			GiveShield(base.info.caster, shieldMissingHpRatio * base.info.caster.Status.missingHealth, float.PositiveInfinity);
			FxPlayNewNetworked(tpStartEffect, base.info.caster);
			Vector3 end = base.info.target.agentPosition + (base.info.target.agentPosition - base.info.caster.agentPosition).normalized * backPosDis;
			Vector3 dest = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, end);
			base.info.caster.Visual.DisableRenderers();
			base.info.caster.Control.StartDaze(appearDuration);
			CreateBasicEffect(base.info.caster, new InvisibleEffect(), appearDuration);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = true,
				canGoOverTerrain = true,
				destination = dest,
				duration = appearDuration,
				ease = DewEase.EaseOutCubic,
				isCanceledByCC = false,
				isFriendly = true,
				onFinish = delegate
				{
					base.info.caster.Control.StartDaze(finishDelay);
					FxPlayNetworked(tpEndEffect, base.info.caster);
					Vector3 positionOnGround = Dew.GetPositionOnGround(dest);
					CreateAbilityInstance<Ai_Mon_LavaLand_FireElemental_DestructSub>(positionOnGround, null, new CastInfo(base.info.caster));
					Destroy();
				},
				onCancel = base.Destroy,
				rotateForward = true
			});
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.info.caster != null)
		{
			base.info.caster.Visual.EnableRenderers();
		}
	}

	private void MirrorProcessed()
	{
	}
}
