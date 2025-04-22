public class Rev_PlayWithRangedTraveler : DewReverieItem
{
	public override int grantedStardust => 20;

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		if (Dew.IsRangedHero(base.hero.classType))
		{
			Complete();
		}
	}
}
