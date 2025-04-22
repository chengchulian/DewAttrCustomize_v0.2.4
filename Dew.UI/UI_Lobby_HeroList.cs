using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby_HeroList : MonoBehaviour
{
	public Transform parentTransform;

	public UI_Lobby_HeroList_Item itemPrefab;

	public UI_Lobby_Loadout_AvailableSkills availableSkills;

	public List<string> heroTypes;

	private List<UI_Lobby_HeroList_Item> _items = new List<UI_Lobby_HeroList_Item>();

	private void Start()
	{
		foreach (string h in heroTypes)
		{
			if (Dew.IsHeroIncludedInGame(h))
			{
				Hero hero = DewResources.GetByShortTypeName<Hero>(h);
				UI_Lobby_HeroList_Item item = Object.Instantiate(itemPrefab, parentTransform);
				item.hero = hero;
				_items.Add(item);
			}
		}
	}

	private int GetCurrentIndex()
	{
		if (ManagerBase<GlobalUIManager>.instance.focused is Component c && c.TryGetComponent<UI_Lobby_HeroList_Item>(out var item))
		{
			return _items.IndexOf(item);
		}
		return _items.FindIndex((UI_Lobby_HeroList_Item i) => i.hero.GetType().Name == DewPlayer.local.selectedHeroType);
	}

	public void PrevHero()
	{
		if (LobbyUIManager.instance.IsState("Character") && !availableSkills.isActiveAndEnabled)
		{
			int currentIndex = GetCurrentIndex();
			if (currentIndex > 0 && _items[currentIndex - 1].GetComponent<IGamepadFocusable>().CanBeFocused())
			{
				ManagerBase<GlobalUIManager>.instance.SimulateClickOnUIElement(_items[currentIndex - 1]);
				ManagerBase<GlobalUIManager>.instance.SetFocus(_items[currentIndex - 1].GetComponent<IGamepadFocusable>());
			}
			else if (currentIndex >= 0)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(_items[currentIndex].GetComponent<IGamepadFocusable>());
			}
		}
	}

	public void NextHero()
	{
		if (LobbyUIManager.instance.IsState("Character") && !availableSkills.isActiveAndEnabled)
		{
			int currentIndex = GetCurrentIndex();
			if (currentIndex + 1 < _items.Count && _items[currentIndex + 1].GetComponent<IGamepadFocusable>().CanBeFocused())
			{
				ManagerBase<GlobalUIManager>.instance.SimulateClickOnUIElement(_items[currentIndex + 1]);
				ManagerBase<GlobalUIManager>.instance.SetFocus(_items[currentIndex + 1].GetComponent<IGamepadFocusable>());
			}
			else if (currentIndex >= 0)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(_items[currentIndex].GetComponent<IGamepadFocusable>());
			}
		}
	}
}
