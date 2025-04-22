using System;
using UnityEngine;

[LogicUpdatePriority(1050)]
public class UI_Lobby_Loadout_ComponentBase : LogicBehaviour
{
	public Hero hero { get; private set; }

	public HeroLoadoutData loadout { get; private set; }

	protected virtual void Start()
	{
		Dew.CallOnReady(this, () => DewPlayer.local != null, delegate
		{
			DewPlayer.local.ClientEvent_OnSelectedHeroTypeChanged += new Action<string>(ClientPlayerEventOnSelectedHeroTypeChanged);
			DewPlayer.local.ClientEvent_OnSelectedLoadoutChanged += new Action<HeroLoadoutData>(ClientPlayerEventOnSelectedLoadoutChanged);
			hero = DewResources.GetByShortTypeName(DewPlayer.local.selectedHeroType) as Hero;
			loadout = DewPlayer.local.selectedLoadout;
			OnHeroChanged();
			OnLoadoutChanged();
		});
	}

	protected virtual void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnSelectedHeroTypeChanged -= new Action<string>(ClientPlayerEventOnSelectedHeroTypeChanged);
			DewPlayer.local.ClientEvent_OnSelectedLoadoutChanged -= new Action<HeroLoadoutData>(ClientPlayerEventOnSelectedLoadoutChanged);
		}
	}

	private void ClientPlayerEventOnSelectedHeroTypeChanged(string obj)
	{
		hero = DewResources.GetByShortTypeName(obj) as Hero;
		try
		{
			OnHeroChanged();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
	}

	private void ClientPlayerEventOnSelectedLoadoutChanged(HeroLoadoutData obj)
	{
		loadout = obj;
		try
		{
			OnLoadoutChanged();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
	}

	protected virtual void OnLoadoutChanged()
	{
	}

	protected virtual void OnHeroChanged()
	{
	}
}
