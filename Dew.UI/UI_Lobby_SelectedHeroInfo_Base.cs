using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Lobby_SelectedHeroInfo_Base : MonoBehaviour, ILangaugeChangedCallback
{
	private bool _didRegister;

	public TextMeshProUGUI text { get; private set; }

	public Hero selectedHero { get; private set; }

	public string selectedHeroName { get; private set; }

	protected virtual void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	protected virtual void OnEnable()
	{
		if (!_didRegister)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			while (DewPlayer.local == null)
			{
				yield return null;
			}
			DewPlayer.local.ClientEvent_OnSelectedHeroTypeChanged += new Action<string>(ClientPlayerEventOnSelectedHeroTypeChanged);
			ClientPlayerEventOnSelectedHeroTypeChanged(null);
			_didRegister = true;
		}
	}

	protected virtual void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnSelectedHeroTypeChanged -= new Action<string>(ClientPlayerEventOnSelectedHeroTypeChanged);
		}
	}

	private void ClientPlayerEventOnSelectedHeroTypeChanged(string obj)
	{
		selectedHeroName = DewPlayer.local.selectedHeroType;
		selectedHero = DewResources.GetByShortTypeName<Hero>(selectedHeroName);
		try
		{
			OnHeroChanged();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
	}

	protected virtual void OnHeroChanged()
	{
	}

	public void OnLanguageChanged()
	{
		OnHeroChanged();
	}
}
