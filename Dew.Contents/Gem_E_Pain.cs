using System;
using UnityEngine;

public class Gem_E_Pain : Gem
{
	[Serializable]
	public class ElementalEffect
	{
		public GameObject hitEffect;

		public GameObject skillEffect;
	}

	public AbilityTargetValidator validator;

	public ScalingValue damageRatio;

	public DewCollider range;

	public float procCoefficientMultiplier = 0.5f;

	[SerializeField]
	private ElementalEffect _fireEffects;

	[SerializeField]
	private ElementalEffect _coldEffects;

	[SerializeField]
	private ElementalEffect _lightEffects;

	[SerializeField]
	private ElementalEffect _darkEffects;

	[SerializeField]
	private ElementalEffect _normalEffects;

	private GameObject _hit;

	private GameObject _effect;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.ActorEvent_OnDealDamage += new Action<EventInfoDamage>(ActorEventOnDealDamage);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && oldSkill != null)
		{
			oldSkill.ActorEvent_OnDealDamage -= new Action<EventInfoDamage>(ActorEventOnDealDamage);
		}
	}

	private void ActorEventOnDealDamage(EventInfoDamage obj)
	{
		if (base.owner == null || base.skill == null || obj.actor == null || !base.owner.CheckEnemyOrNeutral(obj.victim) || obj.chain.DidReact(this))
		{
			return;
		}
		ElementalEffect elementalEffect = obj.damage.elemental switch
		{
			ElementalType.Fire => _fireEffects, 
			ElementalType.Cold => _coldEffects, 
			ElementalType.Light => _lightEffects, 
			ElementalType.Dark => _darkEffects, 
			null => _normalEffects, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		_hit = elementalEffect.hitEffect;
		_effect = elementalEffect.skillEffect;
		FxPlayNewNetworked(_effect, obj.victim.position, Quaternion.identity);
		range.transform.position = obj.victim.position;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, validator, base.owner);
		float value = GetValue(damageRatio);
		ReadOnlySpan<Entity> readOnlySpan = entities;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (!(entity == obj.victim))
			{
				DamageData damageData = obj.actor.DefaultDamage((obj.damage.amount + obj.damage.discardedAmount) * value, obj.damage.procCoefficient * procCoefficientMultiplier).SetAmountOrigin(obj.damage).SetOriginPosition(obj.victim.position)
					.SetSourceType(obj.damage.type);
				if (obj.damage.elemental.HasValue)
				{
					damageData.SetElemental(obj.damage.elemental.Value);
				}
				damageData.Dispatch(entity, obj.chain.New(this));
				FxPlayNewNetworked(_hit, entity);
			}
		}
		handle.Return();
		NotifyUse();
	}

	private void MirrorProcessed()
	{
	}
}
