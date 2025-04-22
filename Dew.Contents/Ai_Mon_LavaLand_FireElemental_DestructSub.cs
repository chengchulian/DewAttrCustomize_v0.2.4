using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_LavaLand_FireElemental_DestructSub : AbilityInstance
{
	public GameObject telegraph;

	public GameObject hitEffect;

	public GameObject explodeEffect;

	public GameObject deathEffect;

	public DewAnimationClip startAnim;

	public DewCollider range;

	public Knockback knockback;

	public float startAnimSpeed;

	public float explodeDuration = 10f;

	public float explodeDelay = 3.5f;

	public ScalingValue dmgFactor;

	public float knockupStrength = 1.5f;

	protected override IEnumerator OnCreateSequenced()
	{
		yield return base.OnCreateSequenced();
		if (!base.isServer)
		{
			yield break;
		}
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), float.PositiveInfinity);
		DestroyOnDeath(base.info.caster);
		yield return new SI.WaitForSeconds(0.1f);
		FxPlayNetworked(telegraph);
		base.info.caster.Animation.PlayAbilityAnimation(startAnim, startAnimSpeed);
		yield return new SI.WaitForSeconds(explodeDelay);
		base.info.caster.Visual.DisableRenderers();
		base.info.caster.Control.StartDaze(explodeDuration);
		FxPlayNetworked(explodeEffect);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			CreateDamage(DamageData.SourceType.Default, dmgFactor).SetElemental(ElementalType.Fire).Dispatch(entity);
			knockback.ApplyWithOrigin(base.info.caster.position, entity);
			FxPlayNewNetworked(hitEffect, entity);
			if (!entity.Status.hasUnstoppable)
			{
				entity.Visual.KnockUp(knockupStrength, isFriendly: false);
			}
		}
		handle.Return();
		CreateAbilityInstance<Ai_Mon_LavaLand_FireElemental_ExplosionSub>(base.position, null, new CastInfo(base.info.caster));
		FxPlayNetworked(deathEffect, base.info.caster);
		base.info.caster.Kill();
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = Dew.GetPositionOnGround(base.info.caster.position);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(telegraph);
		}
	}

	private void MirrorProcessed()
	{
	}
}
