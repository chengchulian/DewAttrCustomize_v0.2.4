using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_BackStep_Instance : AbilityInstance
{
	public DewCollider range;

	public ScalingValue dmgFactor;

	public Knockback Knockback;

	public float castDuration;

	public float postDelay;

	public GameObject fxInstance;

	public GameObject fxTelegraph;

	public GameObject fxHit;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 10f, "ObliviaxUnstoppable").DestroyOnDestroy(this);
			base.info.caster.Control.StartDaze(castDuration + postDelay);
			base.info.caster.Animation.PlayAbilityAnimation(startAnim);
			yield return new SI.WaitForSeconds(0.2f);
			FxPlayNetworked(fxTelegraph, base.info.caster);
			yield return new SI.WaitForSeconds(castDuration);
			FxStopNetworked(fxTelegraph);
			base.info.caster.Animation.PlayAbilityAnimation(endAnim);
			FxPlayNetworked(fxInstance, base.info.caster);
			range.transform.position = base.info.caster.position;
			range.transform.rotation = base.info.caster.rotation;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				FxPlayNewNetworked(fxHit, entity);
				Knockback.ApplyWithOrigin(base.info.caster.position, entity);
				CreateDamage(DamageData.SourceType.Default, dmgFactor).SetDirection(base.info.forward).SetOriginPosition(base.info.caster.agentPosition).SetElemental(ElementalType.Dark)
					.Dispatch(entity);
				CreateBasicEffect(entity, new StunEffect(), 1.25f);
			}
			handle.Return();
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
