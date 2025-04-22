using UnityEngine;

public class St_Q_GoldenBurst : SkillTrigger
{
	public float perStackAmp = 0.2f;

	private Ai_Q_GoldenBurst _goldenBurstPrefab;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
		{
			if (actor is Ai_Q_GoldenBurst && !(base.owner == null) && base.owner.CheckEnemyOrNeutral(target) && !data.IsAmountModifiedBy(this) && base.owner.Status.TryGetStatusEffect<Se_Q_GoldenBurst_Wither>(out var effect) && effect.stack > 0)
			{
				data.SetAttr(DamageAttribute.IsCrit);
				data.ApplyAmplification(perStackAmp * (float)effect.stack);
				data.SetAmountModifiedBy(this);
			}
		});
		dealtHealProcessor.Add(delegate(ref HealData data, Actor actor, Entity target)
		{
			if (actor is Ai_Q_GoldenBurst && !(base.owner == null) && !data.IsAmountModifiedBy(this) && base.owner.Status.TryGetStatusEffect<Se_Q_GoldenBurst_Wither>(out var effect2) && effect2.stack > 0)
			{
				data.SetCrit();
				data.ApplyAmplification(perStackAmp * (float)effect2.stack);
				data.SetAmountModifiedBy(this);
			}
		});
	}

	public override bool CanBeReserved()
	{
		if (_goldenBurstPrefab == null)
		{
			_goldenBurstPrefab = DewResources.GetByType<Ai_Q_GoldenBurst>();
		}
		if (base.CanBeReserved() && base.owner != null)
		{
			return _goldenBurstPrefab.GetSelfDamageAmount(base.owner) < base.owner.currentHealth;
		}
		return false;
	}

	public override bool CanBeCast()
	{
		if (_goldenBurstPrefab == null)
		{
			_goldenBurstPrefab = DewResources.GetByType<Ai_Q_GoldenBurst>();
		}
		if (base.CanBeCast() && base.owner != null)
		{
			return _goldenBurstPrefab.GetSelfDamageAmount(base.owner) < base.owner.currentHealth;
		}
		return false;
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && base.owner != null)
		{
			if (_goldenBurstPrefab == null)
			{
				_goldenBurstPrefab = DewResources.GetByType<Ai_Q_GoldenBurst>();
			}
			float selfDamageAmount = _goldenBurstPrefab.GetSelfDamageAmount(base.owner);
			base.owner.Status.specialFill = new Vector2(base.owner.currentHealth - selfDamageAmount, base.owner.currentHealth);
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer && formerOwner != null)
		{
			formerOwner.Status.specialFill = Vector2.zero;
		}
	}

	private void MirrorProcessed()
	{
	}
}
