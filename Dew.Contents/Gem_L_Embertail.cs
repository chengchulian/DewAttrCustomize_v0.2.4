using System;
using UnityEngine;

public class Gem_L_Embertail : Gem
{
	public DewCollider burnCheckRange;

	public AbilityTargetValidator burnCheckable;

	public DewCollider bounceRange;

	public AbilityTargetValidator hittable;

	public GameObject attackEffect;

	public ScalingValue ampPerBurning;

	public ScalingValue skillHastePerBurning;

	public float burningCheckInterval = 0.4f;

	public int critThresholdEnemies;

	private float _lastCheckTime;

	private int _burningEnemies;

	private SkillBonus _bonus;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(SpawnBouncingFire);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && !(oldOwner == null))
		{
			oldOwner.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(SpawnBouncingFire);
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtDamageProcessor.Add(AmpDamage);
			newSkill.dealtHealProcessor.Add(AmpHeal);
			_bonus = new SkillBonus();
			newSkill.AddSkillBonus(_bonus);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && !(oldSkill == null))
		{
			oldSkill.dealtDamageProcessor.Remove(AmpDamage);
			oldSkill.dealtHealProcessor.Remove(AmpHeal);
			oldSkill.RemoveSkillBonus(_bonus);
			_bonus = null;
		}
	}

	private void AmpDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this) && _burningEnemies > 0)
		{
			float value = GetValue(ampPerBurning) * (float)_burningEnemies;
			if (_burningEnemies >= critThresholdEnemies)
			{
				data.SetAttr(DamageAttribute.IsCrit);
			}
			data.ApplyAmplification(value);
			data.SetAmountModifiedBy(this);
		}
	}

	private void AmpHeal(ref HealData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this) && _burningEnemies > 0)
		{
			float value = GetValue(ampPerBurning) * (float)_burningEnemies;
			if (_burningEnemies >= critThresholdEnemies)
			{
				data.SetCrit();
			}
			data.ApplyAmplification(value);
			data.SetAmountModifiedBy(this);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || base.owner == null || _bonus == null || !(Time.time - _lastCheckTime > burningCheckInterval))
		{
			return;
		}
		base.transform.position = base.owner.agentPosition;
		_lastCheckTime = Time.time;
		int num = 0;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = burnCheckRange.GetEntities(out handle, burnCheckable, base.owner);
		for (int i = 0; i < entities.Length; i++)
		{
			if (entities[i].Status.fireStack > 0)
			{
				num++;
			}
		}
		handle.Return();
		_burningEnemies = Mathf.Max(num, _burningEnemies - 1);
		_bonus.cooldownMultiplier = 100f / (100f + GetValue(skillHastePerBurning) * (float)_burningEnemies);
		base.numberDisplay = _burningEnemies;
	}

	private void SpawnBouncingFire(EventInfoAttackEffect obj)
	{
		if (obj.chain.DidReact(this))
		{
			return;
		}
		FxPlayNewNetworked(attackEffect, obj.victim);
		bounceRange.transform.position = obj.victim.position;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = bounceRange.GetEntities(out handle, hittable, base.owner);
		if (entities.Length > 0)
		{
			Entity entity = entities[global::UnityEngine.Random.Range(0, entities.Length)];
			if (entities.Length > 2)
			{
				if (entity == obj.victim)
				{
					entity = entities[global::UnityEngine.Random.Range(0, entities.Length)];
				}
				if (entity == obj.victim)
				{
					entity = entities[global::UnityEngine.Random.Range(0, entities.Length)];
				}
				if (entity == obj.victim)
				{
					entity = entities[global::UnityEngine.Random.Range(0, entities.Length)];
				}
			}
			CreateAbilityInstance(obj.victim.Visual.GetCenterPosition(), null, new CastInfo(base.owner, entity), delegate(Ai_Gem_L_Embertail_Projectile p)
			{
				p.chain = obj.chain.New(this);
				p._strengthMultiplier = obj.strength;
			});
		}
		handle.Return();
		NotifyUse();
	}

	private void MirrorProcessed()
	{
	}
}
