public class At_Mon_Forest_BossDemon_MainSkill : AbilityTrigger
{
	public override void OnCastStart(int configIndex, CastInfo info)
	{
		base.OnCastStart(configIndex, info);
		CreateBasicEffect(base.owner, new UnstoppableEffect(), configs[configIndex].channel.duration);
	}

	public override AbilityInstance OnCastComplete(int configIndex, CastInfo info)
	{
		AbilityInstance result = base.OnCastComplete(configIndex, info);
		base.currentConfigIndex = ((base.currentConfigIndex == 0) ? 1 : 0);
		SetChargeAll(0);
		return result;
	}

	private void MirrorProcessed()
	{
	}
}
