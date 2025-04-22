using System;

public class UI_EntityProvider_LocalHero : UI_EntityProvider
{
	private void Start()
	{
		GameManager.CallOnReady(delegate
		{
			UpdateTarget();
			DewPlayer.local.ClientEvent_OnHeroChanged += new Action<Hero, Hero>(ClientEventOnHeroChanged);
		});
	}

	private void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnHeroChanged -= new Action<Hero, Hero>(ClientEventOnHeroChanged);
		}
	}

	private void ClientEventOnHeroChanged(Hero arg1, Hero arg2)
	{
		UpdateTarget();
	}

	private void UpdateTarget()
	{
		if (!(DewPlayer.local == null) && !(DewPlayer.local.hero == null))
		{
			base.target = DewPlayer.local.hero;
		}
	}
}
