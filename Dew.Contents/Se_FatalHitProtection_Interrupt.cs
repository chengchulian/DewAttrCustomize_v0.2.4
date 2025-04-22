public class Se_FatalHitProtection_Interrupt : StackedStatusEffect
{
	public float invulTime { get; set; }

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		DoDeathInterrupt(delegate
		{
			base.victim.Status.SetHealth(base.victim.Status.maxHealth * 0.25f);
			if (!base.victim.Status.HasStatusEffect<Se_FatalHitProtection_Invulnerable>())
			{
				CreateStatusEffect(base.victim, delegate(Se_FatalHitProtection_Invulnerable se)
				{
					se.duration = invulTime;
				});
				Dew.CallDelayed(delegate
				{
					if (!this.IsNullOrInactive())
					{
						RemoveStack();
					}
				});
			}
		}, 0);
	}

	private void MirrorProcessed()
	{
	}
}
