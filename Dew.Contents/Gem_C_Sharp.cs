public class Gem_C_Sharp : Gem
{
	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		CreateStatusEffectWithSource<Se_Gem_C_Sharp_ArrowSpawner>(info.instance, base.owner, new CastInfo(base.owner));
		NotifyUse();
	}

	private void MirrorProcessed()
	{
	}
}
