using System;
using System.Linq;
using UnityEngine;

public class Gem_L_Paranoia : Gem
{
	public ScalingValue skillHaste;

	public float searchInterval = 0.25f;

	public DewCollider range;

	public float duration;

	public GameObject casterEffect;

	private float _lastSearchTime;

	private SkillBonus _bonus;

	private bool _isEnemyInRange;

	private float _lastDamageTime = float.NegativeInfinity;

	public float reducedRatio => 1f - 1f / (1f + GetValue(skillHaste) * 0.01f);

	public override bool IsReady()
	{
		if (base.IsReady() && _bonus != null)
		{
			return _bonus.cooldownMultiplier < 0.99f;
		}
		return false;
	}

	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		NotifyUse();
	}

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(EntityEventOnTakeDamage);
		}
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
		if (!(obj.damage.amount < 0.9f))
		{
			_lastDamageTime = Time.time;
			NotifyUse();
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			if (oldOwner != null)
			{
				oldOwner.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(EntityEventOnTakeDamage);
			}
			FxStopNetworked(casterEffect);
			_lastDamageTime = float.NegativeInfinity;
			base.numberDisplay = 0;
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			_bonus = new SkillBonus();
			_bonus.cooldownMultiplier = 1f - reducedRatio;
			newSkill.AddSkillBonus(_bonus);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			if (oldSkill != null)
			{
				oldSkill.RemoveSkillBonus(_bonus);
			}
			_bonus = null;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || base.owner == null || base.skill == null)
		{
			return;
		}
		base.numberDisplay = Mathf.CeilToInt(Mathf.Clamp(duration - (Time.time - _lastDamageTime), 0f, duration));
		if (Time.time - _lastDamageTime > duration)
		{
			if ((double)_bonus.cooldownMultiplier < 0.9)
			{
				_bonus.cooldownMultiplier = 1f;
				FxStopNetworked(casterEffect);
			}
			return;
		}
		if (_bonus != null && Math.Abs(_bonus.cooldownMultiplier - (1f - reducedRatio)) > 0.001f)
		{
			_bonus.cooldownMultiplier = 1f - reducedRatio;
			FxPlayNetworked(casterEffect, base.owner);
		}
		if (Time.time - _lastSearchTime > searchInterval)
		{
			base.transform.position = base.owner.agentPosition;
			_lastSearchTime = Time.time;
			_isEnemyInRange = range.GetEntities(out var handle, (Entity e) => base.owner.GetRelation(e) == EntityRelation.Enemy).Length > 0;
			handle.Return();
		}
		if (!_isEnemyInRange || base.owner.IsNullInactiveDeadOrKnockedOut() || base.owner.Control.ongoingChannels.Count > 0 || !base.skill.CanBeCast() || !base.skill.CanBeReserved() || base.owner.Control.queuedActions.Any((ActionBase c) => c is ActionCast actionCast && actionCast.trigger == base.skill) || NetworkedManagerBase<ZoneManager>.instance.isInAnyTransition)
		{
			return;
		}
		CastInfo info;
		if (base.skill.currentConfig.castMethod.type == CastMethodType.None)
		{
			info = new CastInfo(base.owner);
		}
		else
		{
			Entity entity = null;
			ArrayReturnHandle<Entity> handle2;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle2, base.transform.position, base.skill.currentConfig.castMethod.GetEffectiveRange(), new CollisionCheckSettings
			{
				sortComparer = CollisionCheckSettings.DistanceFromCenter
			});
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				Entity entity2 = readOnlySpan[i];
				if (base.skill.currentConfig.targetValidator.Evaluate(base.owner, entity2))
				{
					entity = entity2;
					break;
				}
			}
			handle2.Return();
			if (entity == null)
			{
				return;
			}
			info = base.skill.GetPredictedCastInfoToTarget(entity, global::UnityEngine.Random.Range(0f, 0.5f));
		}
		base.owner.Control.Cast(base.skill, base.skill.currentConfigIndex, info);
	}

	protected override void OnQualityChange(int oldQuality, int newQuality)
	{
		base.OnQualityChange(oldQuality, newQuality);
		if (base.isServer && _bonus != null)
		{
			_bonus.cooldownMultiplier = 1f - reducedRatio;
		}
	}

	private void MirrorProcessed()
	{
	}
}
