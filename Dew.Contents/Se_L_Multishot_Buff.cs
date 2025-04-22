using System;
using System.Collections;
using UnityEngine;

public class Se_L_Multishot_Buff : StatusEffect
{
	public ScalingValue hasteAmount;

	public ScalingValue speedAmount;

	public DewCollider subTargetRange;

	public int arrowCount = 3;

	public float firstArrowDelay = 0.1f;

	public float arrowInterval = 0.075f;

	public float duration = 6f;

	public bool onlyFirstArrowAttackEffect;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoHaste(GetValue(hasteAmount));
			DoSpeed(GetValue(speedAmount));
			base.victim.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
			SetTimer(duration);
			ShowOnScreenTimer();
			base.info.caster.Ability.attackAbility.ResetCooldown();
		}
	}

	private void EntityEventOnAttackEffectTriggered(EventInfoAttackEffect obj)
	{
		StartSequence(ShootArrowSequence(obj));
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.victim == null))
		{
			base.victim.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
		}
	}

	private IEnumerator ShootArrowSequence(EventInfoAttackEffect obj)
	{
		if (obj.chain.DidReact(this, checkOnlyType: true))
		{
			yield break;
		}
		Entity mainTarget = obj.victim;
		yield return new SI.WaitForSeconds(firstArrowDelay);
		for (int i = 0; i < arrowCount; i++)
		{
			if (mainTarget == null)
			{
				break;
			}
			subTargetRange.transform.position = mainTarget.position;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = subTargetRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
			{
				sortComparer = CollisionCheckSettings.Random
			});
			Entity entity = mainTarget;
			if (entities.Length > 0)
			{
				entity = entities[0];
			}
			handle.Return();
			if (entity == null)
			{
				break;
			}
			bool triggersAttackEffect = !onlyFirstArrowAttackEffect || i == 0;
			CreateAbilityInstance(base.info.caster.position, Quaternion.identity, new CastInfo(base.info.caster, entity), delegate(Ai_L_Multishot_Arrow a)
			{
				a.strength = obj.strength;
				a.triggerAttackEffect = triggersAttackEffect;
				a.chain = obj.chain.New(this);
			});
			yield return new SI.WaitForSeconds(arrowInterval);
		}
	}

	private void MirrorProcessed()
	{
	}
}
