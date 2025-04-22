using System;
using System.Collections;
using UnityEngine;

public class UI_Lobby_Loadout : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (DewPlayer.local == null)
			{
				yield return null;
			}
			DewPlayer.local.ClientEvent_OnSelectedLoadoutChanged += new Action<HeroLoadoutData>(LoadoutChanged);
		}
	}

	private void LoadoutChanged(HeroLoadoutData obj)
	{
	}
}
