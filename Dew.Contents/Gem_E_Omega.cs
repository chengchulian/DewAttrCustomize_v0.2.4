using UnityEngine;

public class Gem_E_Omega : Gem
{
	public int maxChargeCount;

	public ScalingValue maxChargeAmount;

	public GameObject fxCharge;

	public GameObject fxChargeComplete;

	public GameObject fxActivate;

	public GameObject fxActivateMax;

	public GameObject fxCast;

	public GameObject fxCastMax;

	private float _currentChargeRate;

	private float _lastChargeTime;

	public float chargeRate => GetValue(maxChargeAmount) / (float)maxChargeCount;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			_lastChargeTime = 0f;
			_currentChargeRate = 0f;
			base.numberDisplay = 0;
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			_lastChargeTime = 0f;
			_currentChargeRate = 0f;
			base.numberDisplay = 0;
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			_lastChargeTime = 0f;
			_currentChargeRate = 0f;
			base.numberDisplay = 0;
		}
	}

	protected override void OnCastCompleteBeforePrepare(EventInfoCast info)
	{
		base.OnCastCompleteBeforePrepare(info);
		if (base.owner == null || !info.trigger.configs[info.configIndex].canConsumeCastBonus)
		{
			return;
		}
		bool isFullyCharged = base.numberDisplay == maxChargeCount;
		FxPlayNewNetworked(isFullyCharged ? fxCastMax : fxCast, base.owner);
		float amp = _currentChargeRate;
		info.instance.dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
		{
			if (base.isValid && !data.IsAmountModifiedBy(this) && !(base.owner == null) && base.owner.CheckEnemyOrNeutral(target))
			{
				data.ApplyAmplification(amp);
				data.SetAttr(DamageAttribute.IsCrit);
				data.SetAmountModifiedBy(this);
				GameObject effect = (isFullyCharged ? fxActivateMax : fxActivate);
				FxPlayNewNetworked(effect, target);
			}
		});
		NotifyUse();
		_currentChargeRate = 0f;
		_lastChargeTime = 0f;
		base.numberDisplay = 0;
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		_lastChargeTime += dt;
		if (_lastChargeTime < 0.99f)
		{
			return;
		}
		_lastChargeTime = 0f;
		if (base.owner.IsNullInactiveDeadOrKnockedOut() || base.skill == null || base.skill.currentCharges[0] <= 0)
		{
			return;
		}
		float value = GetValue(maxChargeAmount);
		if (!(_currentChargeRate >= value))
		{
			FxPlayNewNetworked(fxCharge, base.owner);
			_currentChargeRate += chargeRate;
			base.numberDisplay = Mathf.FloorToInt(_currentChargeRate / GetValue(maxChargeAmount) * (float)maxChargeCount);
			if (_currentChargeRate >= value)
			{
				FxPlayNewNetworked(fxChargeComplete, base.owner);
				base.numberDisplay = maxChargeCount;
				_currentChargeRate = value;
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
