using System;
using System.Collections;
using UnityEngine;

public class Gem_E_Thunder : Gem
{
	public GameObject chargeEffect;

	public GameObject startEffect;

	public float delay;

	public float randomMagnitude;

	public float range;

	public float interval;

	public int noTargetGraceCount;

	public ScalingValue maxCharge;

	private int _currentCharge;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(ClientHeroEventOnSkillUse);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			if (oldOwner != null)
			{
				oldOwner.ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(ClientHeroEventOnSkillUse);
			}
			_currentCharge = 0;
			base.numberDisplay = 0;
		}
	}

	private void ClientHeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		if (base.isValid && !(obj.skill == base.skill))
		{
			int num = Mathf.RoundToInt(GetValue(maxCharge));
			if (_currentCharge < num)
			{
				_currentCharge = Mathf.Clamp(_currentCharge + 1, 0, num);
				base.numberDisplay = _currentCharge;
				NotifyUse();
				FxPlayNewNetworked(chargeEffect, base.owner);
			}
		}
	}

	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		int count;
		if (_currentCharge > 0)
		{
			FxPlayNewNetworked(startEffect, base.owner);
			count = _currentCharge;
			StartCoroutine(Routine());
			_currentCharge = 0;
			base.numberDisplay = 0;
			NotifyUse();
		}
		IEnumerator Routine()
		{
			info.instance.LockDestroy();
			yield return new WaitForSeconds(delay);
			int currentStrikes = 0;
			for (int i = 0; i < count; i++)
			{
				if (!base.isValid)
				{
					info.instance.UnlockDestroy();
					yield break;
				}
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.owner.position, range, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
				{
					sortComparer = CollisionCheckSettings.Random
				});
				if (readOnlySpan.Length > 0)
				{
					Vector3 positionOnGround = Dew.GetPositionOnGround(readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)].agentPosition + global::UnityEngine.Random.insideUnitSphere * randomMagnitude);
					CreateAbilityInstanceWithSource<Ai_Gem_E_Thunder>(info.instance, positionOnGround, null, new CastInfo(base.owner));
					currentStrikes = 0;
				}
				else
				{
					currentStrikes++;
					if (currentStrikes <= noTargetGraceCount)
					{
						i--;
					}
				}
				handle.Return();
				NotifyUse();
				yield return new WaitForSeconds(interval);
			}
			info.instance.UnlockDestroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
