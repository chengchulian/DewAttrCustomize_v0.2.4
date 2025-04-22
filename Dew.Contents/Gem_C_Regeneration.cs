public class Gem_C_Regeneration : Gem
{
	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		if (base.owner.isInCombat)
		{
			if (base.owner.Status.TryGetStatusEffect<Se_Gem_Regeneration>(out var effect))
			{
				effect.Destroy();
			}
			CreateStatusEffectWithSource<Se_Gem_Regeneration>(info.instance, base.owner, new CastInfo(base.owner));
			NotifyUse();
		}
	}

	private void MirrorProcessed()
	{
	}
}
