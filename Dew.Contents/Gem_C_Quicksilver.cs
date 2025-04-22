public class Gem_C_Quicksilver : Gem
{
	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		if (base.owner.Status.TryGetStatusEffect<Se_Gem_C_Quicksilver>(out var effect))
		{
			effect.ResetTimer();
		}
		else
		{
			CreateStatusEffect<Se_Gem_C_Quicksilver>(base.owner, new CastInfo(base.owner));
		}
		NotifyUse();
	}

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (info.damage.elemental == ElementalType.Fire)
		{
			if (base.owner.Status.TryGetStatusEffect<Se_Gem_C_Quicksilver>(out var effect))
			{
				effect.ResetTimer();
			}
			else
			{
				CreateStatusEffect<Se_Gem_C_Quicksilver>(base.owner, new CastInfo(base.owner));
			}
			NotifyUse();
		}
	}

	private void MirrorProcessed()
	{
	}
}
