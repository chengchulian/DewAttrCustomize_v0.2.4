public class Se_AcceleratedTime : StatusEffect
{
	public float dazeDuration = 0.75f;

	public StatBonus heroStatBonus;

	public StatBonus monsterStatbonus;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.Control.StartDaze(dazeDuration);
			if (base.victim is Monster)
			{
				DoStatBonus(monsterStatbonus);
			}
			else if (base.victim is Hero)
			{
				DoStatBonus(heroStatBonus);
			}
			else
			{
				Destroy();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
