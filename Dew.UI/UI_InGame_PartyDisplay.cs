using System;
using System.Collections;
using UnityEngine;

public class UI_InGame_PartyDisplay : MonoBehaviour
{
	public UI_InGame_PartyDisplay_Item itemPrefab;

	public Transform itemParent;

	public bool showSelf;

	private void Start()
	{
		NetworkedManagerBase<ActorManager>.instance.onHeroAdd += new Action<Hero>(HandleNewHero);
		foreach (Hero h in NetworkedManagerBase<ActorManager>.instance.allHeroes)
		{
			HandleNewHero(h);
		}
	}

	private void HandleNewHero(Hero h)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.1f);
			if (showSelf || !h.isOwned)
			{
				global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).GetComponent<UI_EntityProvider>().target = h;
			}
		}
	}
}
