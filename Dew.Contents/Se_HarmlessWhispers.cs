using UnityEngine;

public class Se_HarmlessWhispers : StatusEffect
{
	public float healthBonusPercentage = 15f;

	public float powerBonusPercentage = 10f;

	public GameObject fxMirageSkinTelegraph;

	private StatBonus _bonus;

	private int _anger;

	private float _lastAngerTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		while (true)
		{
			Se_GenericEffectContainer se_GenericEffectContainer = base.victim.Status.FindStatusEffect((Se_GenericEffectContainer s) => s.effect is UnstoppableEffect);
			if (se_GenericEffectContainer == null)
			{
				break;
			}
			se_GenericEffectContainer.Destroy();
		}
		_bonus = DoStatBonus(new StatBonus
		{
			maxHealthPercentage = healthBonusPercentage,
			abilityPowerPercentage = powerBonusPercentage,
			attackDamagePercentage = powerBonusPercentage,
			tenacityFlat = 50f + 5f * NetworkedManagerBase<GameManager>.instance.GetMultiplayerDifficultyFactor(reduceWhenDead: true)
		});
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		if ((base.victim.Status.hasStun || base.victim.Status.hasSilence || base.victim.Status.hasRoot || base.victim.Control.isAirborne) && Time.time - _lastAngerTime > 0.75f)
		{
			_lastAngerTime = Time.time;
			_anger++;
		}
		if (_anger > 6)
		{
			_anger = 0;
			_lastAngerTime = Time.time + 2.5f;
			if (base.victim.Status.currentHealth / base.victim.Status.maxHealth > 0.35f)
			{
				CreateStatusEffect<Se_HarmlessWhispers_GrantShield>(base.victim);
			}
		}
		if (base.victim.Status.hasCrowdControlImmunity)
		{
			_lastAngerTime = Time.time;
		}
	}

	private void MirrorProcessed()
	{
	}
}
