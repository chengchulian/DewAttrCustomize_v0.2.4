public class Gem_C_Wind : Gem
{
	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		if (base.owner.Status.TryGetStatusEffect<Se_Gem_C_Wind>(out var effect))
		{
			effect.Destroy();
		}
		CreateStatusEffectWithSource<Se_Gem_C_Wind>(info.instance, base.owner, new CastInfo(base.owner));
		NotifyUse();
	}

	private void MirrorProcessed()
	{
	}
}
