using System;

public class Mon_DarkCave_SeekerHallucination : Monster, IForceHeroicHealthbar
{
	[NonSerialized]
	public float destroyHpThreshold;

	protected override void OnCreate()
	{
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
	}

	private void MirrorProcessed()
	{
	}
}
