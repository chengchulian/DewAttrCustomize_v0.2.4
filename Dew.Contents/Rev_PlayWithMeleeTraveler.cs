public class Rev_PlayWithMeleeTraveler : DewReverieItem
{
	public override int grantedStardust => 20;

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		if (Dew.IsMeleeHero(base.hero.classType))
		{
			Complete();
		}
	}
}
