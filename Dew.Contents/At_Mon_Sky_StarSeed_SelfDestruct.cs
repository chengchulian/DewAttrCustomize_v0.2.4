public class At_Mon_Sky_StarSeed_SelfDestruct : AbilityTrigger
{
	public bool doUnstoppable;

	private StatusEffect _uns;

	public override void OnCastStart(int configIndex, CastInfo info)
	{
		base.OnCastStart(configIndex, info);
		if (doUnstoppable)
		{
			_uns = CreateBasicEffect(info.caster, new UnstoppableEffect(), 1000f, "starseed_unstoppable");
		}
	}

	protected override void OnCastCancel(int configIndex, CastInfo info)
	{
		base.OnCastCancel(configIndex, info);
		if (_uns != null)
		{
			_uns.Destroy();
		}
	}

	public override AbilityInstance OnCastComplete(int configIndex, CastInfo info)
	{
		if (_uns != null)
		{
			_uns.Destroy();
		}
		return base.OnCastComplete(configIndex, info);
	}

	private void MirrorProcessed()
	{
	}
}
