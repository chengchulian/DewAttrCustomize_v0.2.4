public class Se_R_Cataclysm : StatusEffect
{
	public ScalingValue shieldAmount;

	public float duration;

	public ScalingValue bonusApPercent;

	public int reuseCount = 3;

	public bool doUnstoppable;

	private int _currentUseCount;

	private AbilityTrigger.ChangedConfigHandle _handle;

	private StatBonus _bonus;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_handle = base.firstTrigger.ChangeConfigTimed(1, duration, OnUse, base.Destroy);
			GiveShield(base.victim, GetValue(shieldAmount), duration);
			SetTimer(duration);
			ShowOnScreenTimer();
			if (doUnstoppable)
			{
				DoUnstoppable();
			}
			_bonus = new StatBonus
			{
				abilityPowerPercentage = GetValue(bonusApPercent)
			};
			base.victim.Status.AddStatBonus(_bonus);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			if (_handle.isActive)
			{
				_handle.Stop();
			}
			if (base.victim != null && _bonus != null)
			{
				base.victim.Status.RemoveStatBonus(_bonus);
			}
		}
	}

	private void OnUse(EventInfoAbilityInstance obj)
	{
		CreateAbilityInstance<Ai_R_Cataclysm_Meteor>(obj.instance.info.point, null, new CastInfo(base.info.caster));
		_currentUseCount++;
		if (_currentUseCount >= reuseCount && _handle.isActive)
		{
			_handle.Stop();
		}
	}

	private void MirrorProcessed()
	{
	}
}
