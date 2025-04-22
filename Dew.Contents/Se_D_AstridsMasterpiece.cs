using System;
using UnityEngine;

public class Se_D_AstridsMasterpiece : StatusEffect
{
	public ScalingValue damage;

	public ScalingValue shieldAmount;

	public float shieldDuration = 1.5f;

	public float radius = 9f;

	public float checkInterval = 0.5f;

	public GameObject[] hitEffectsByCharge;

	public GameObject fxActivateOnSelf;

	public bool setCritMarker;

	private float _lastCheckTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(CheckExposed);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(CheckExposed);
		}
	}

	private void CheckExposed(EventInfoAttackEffect obj)
	{
		Se_D_AstridsMasterpiece_Exposed se_D_AstridsMasterpiece_Exposed = null;
		foreach (StatusEffect statusEffect in obj.victim.Status.statusEffects)
		{
			if (statusEffect is Se_D_AstridsMasterpiece_Exposed se_D_AstridsMasterpiece_Exposed2 && statusEffect.parentActor == this)
			{
				se_D_AstridsMasterpiece_Exposed = se_D_AstridsMasterpiece_Exposed2;
				break;
			}
		}
		if (!(se_D_AstridsMasterpiece_Exposed == null) && se_D_AstridsMasterpiece_Exposed.chargeCount > 0)
		{
			se_D_AstridsMasterpiece_Exposed.chargeCount--;
			DamageData damageData = Damage(damage).ApplyStrength(obj.strength).SetOriginPosition(base.victim.position);
			if (setCritMarker)
			{
				damageData.SetAttr(DamageAttribute.IsCrit);
			}
			damageData.Dispatch(obj.victim);
			GiveShield(base.victim, GetValue(shieldAmount) * obj.strength, shieldDuration, isDecay: true);
			FxPlayNewNetworked(fxActivateOnSelf, base.victim);
			FxPlayNewNetworked(hitEffectsByCharge[se_D_AstridsMasterpiece_Exposed.chargeCount], obj.victim);
			if (base.victim is Hero hero && hero.Skill.Q is St_Q_Lunge st_Q_Lunge)
			{
				st_Q_Lunge.ApplyCooldownReduction(st_Q_Lunge.reducedCooldownByMeister * obj.strength);
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && Time.time - _lastCheckTime > checkInterval)
		{
			_lastCheckTime = Time.time;
			CheckAndApplyExposed();
		}
	}

	private void CheckAndApplyExposed()
	{
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.victim.agentPosition, radius, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (entity.Visual.isSpawning || entity.Status.hasInvulnerable)
			{
				continue;
			}
			bool flag = false;
			foreach (StatusEffect statusEffect in entity.Status.statusEffects)
			{
				if (statusEffect is Se_D_AstridsMasterpiece_Exposed && statusEffect.parentActor == this)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				CreateStatusEffect<Se_D_AstridsMasterpiece_Exposed>(entity, new CastInfo(base.victim));
			}
		}
		handle.Return();
	}

	private void MirrorProcessed()
	{
	}
}
