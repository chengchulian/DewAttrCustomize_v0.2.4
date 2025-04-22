public class Se_Q_GoldenBurst_Wither : StackedStatusEffect
{
	private AbilityTrigger _trigger;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.Visual.genericStackIndicatorMax = maxStack;
			base.victim.Visual.genericStackIndicatorValue = base.stack;
			ShowOnScreenTimer("Q_GoldenBurst_StatusEffect_Wither");
			_trigger = base.firstTrigger;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !_trigger.IsNullOrInactive() && base.stack > 0)
		{
			_trigger.fillAmount = base.remainingDecayTime / decayTime;
		}
	}

	protected override void OnStackChange(int oldStack, int newStack)
	{
		base.OnStackChange(oldStack, newStack);
		if (base.isServer)
		{
			base.victim.Visual.genericStackIndicatorValue = newStack;
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			if (base.victim != null)
			{
				base.victim.Visual.genericStackIndicatorValue = 0;
			}
			if (!_trigger.IsNullOrInactive())
			{
				_trigger.fillAmount = 0f;
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
