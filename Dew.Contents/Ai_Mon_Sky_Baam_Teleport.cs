using System;
using UnityEngine;

public class Ai_Mon_Sky_Baam_Teleport : AbilityInstance
{
	public DewCollider Range;

	public AbilityTargetValidator hittable;

	public Knockback Knockback;

	public ScalingValue dmgFactor;

	public Vector3 targetPos;

	public DewEase ease;

	public GameObject tpStartEffect;

	public GameObject tpEndEffect;

	public GameObject hitEffect;

	public float appearDuration;

	public float finishDelay = 2f;

	public float runChance = 0.5f;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			FxPlayNewNetworked(tpStartEffect, base.info.caster);
			FxPlayNewNetworked(startEffect, base.info.caster);
			base.info.caster.Control.StartDaze(appearDuration);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = true,
				canGoOverTerrain = true,
				destination = targetPos,
				duration = appearDuration,
				ease = ease,
				isCanceledByCC = false,
				isFriendly = true,
				onFinish = delegate
				{
					OnTeleportFinished();
					Destroy();
				},
				onCancel = base.Destroy,
				rotateForward = true
			});
		}
	}

	private void OnTeleportFinished()
	{
		FxPlayNetworked(tpEndEffect, base.info.caster);
		base.info.caster.Control.StartDaze(finishDelay);
		Range.transform.position = base.info.caster.position;
		Range.transform.rotation = base.info.caster.rotation;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = Range.GetEntities(out handle, hittable, base.info.caster);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			CreateDamage(DamageData.SourceType.Default, dmgFactor).SetElemental(ElementalType.Light).Dispatch(entity);
			Knockback.ApplyWithOrigin(base.info.caster.position, entity);
			FxPlayNewNetworked(hitEffect, entity);
		}
		handle.Return();
		if (base.info.caster is Mon_Sky_Baam mon_Sky_Baam && global::UnityEngine.Random.value < runChance)
		{
			mon_Sky_Baam.StartRunning();
		}
	}

	private void MirrorProcessed()
	{
	}
}
