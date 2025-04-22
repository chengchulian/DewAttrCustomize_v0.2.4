public class Mon_Polaris : Monster, IForceHeroicHealthbar
{
	protected override DewPlayer defaultOwner => DewPlayer.environment;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			CreateStatusEffect<Se_OntologicalShield>(this, new CastInfo(this));
			CreateBasicEffect(this, new InvisibleEffect(), float.PositiveInfinity);
		}
	}

	private void MirrorProcessed()
	{
	}
}
