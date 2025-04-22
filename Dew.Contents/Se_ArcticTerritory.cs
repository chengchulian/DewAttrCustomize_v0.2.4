public class Se_ArcticTerritory : StatusEffect
{
	public float duration;

	public float slowAmount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			SetTimer(duration);
			DoSlow(slowAmount);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			ApplyElemental(ElementalType.Cold, base.victim);
		}
	}

	private void MirrorProcessed()
	{
	}
}
