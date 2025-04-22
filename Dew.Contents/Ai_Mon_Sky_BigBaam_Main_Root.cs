using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Sky_BigBaam_Main_Root : AbilityInstance
{
	public DewCollider range;

	public bool destroyOnCasterDeath;

	public float checkInterval;

	public float slowDelay;

	public float explodeDelay;

	public ScalingValue explodeDamage;

	public GameObject explodeEffect;

	public GameObject explodeHitEffect;

	public bool immediatelyAttackOnHit;

	private float _lastCheckInterval = float.NegativeInfinity;

	protected override IEnumerator OnCreateSequenced()
	{
		base.position = base.info.point;
		if (!base.isServer)
		{
			yield break;
		}
		if (destroyOnCasterDeath)
		{
			DestroyOnDeath(base.info.caster);
		}
		yield return new SI.WaitForSeconds(explodeDelay);
		FxPlayNetworked(explodeEffect);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			Damage(explodeDamage).SetElemental(ElementalType.Light).SetOriginPosition(base.position).Dispatch(entity);
			if (entity.Status.TryGetStatusEffect<Se_Mon_Sky_BigBaam_Main_Root_Slowed>(out var effect))
			{
				effect.Destroy();
			}
			CreateStatusEffect<Se_Mon_Sky_BigBaam_Main_Root_Rooted>(entity);
			FxPlayNewNetworked(explodeHitEffect, entity);
		}
		if (immediatelyAttackOnHit && entities.Length > 0 && !base.info.caster.IsNullInactiveDeadOrKnockedOut())
		{
			Entity target = entities[global::UnityEngine.Random.Range(0, entities.Length)];
			base.info.caster.Ability.attackAbility.ResetCooldown();
			base.info.caster.Control.Attack(target, doChase: true);
		}
		handle.Return();
		Destroy();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || Time.time - _lastCheckInterval < checkInterval || Time.time - base.creationTime < slowDelay)
		{
			return;
		}
		_lastCheckInterval = Time.time;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			if (entity.Status.TryGetStatusEffect<Se_Mon_Sky_BigBaam_Main_Root_Slowed>(out var effect))
			{
				effect.ResetTimer();
			}
			else
			{
				CreateStatusEffect<Se_Mon_Sky_BigBaam_Main_Root_Slowed>(entity);
			}
		}
		handle.Return();
	}

	private void MirrorProcessed()
	{
	}
}
