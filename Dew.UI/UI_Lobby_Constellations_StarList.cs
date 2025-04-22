using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Lobby_Constellations_StarList : SingletonBehaviour<UI_Lobby_Constellations_StarList>
{
	public UI_Lobby_Constellations_StarItem itemPrefab;

	public RectTransform characterSpecificGroup;

	public TextMeshProUGUI characterSpecificTitle;

	public RectTransform globalGroup;

	public TextMeshProUGUI globalTitle;

	public GameObject seperatorObject;

	public UI_ToggleGroup categoryGroup;

	public UI_ToggleGroup listGroup;

	private int _hoveredIndex = -1;

	public SafeAction onHoveredIndexChanged;

	public SafeAction onSelectedIndexChanged;

	[NonSerialized]
	public List<UI_Lobby_Constellations_StarItem> items = new List<UI_Lobby_Constellations_StarItem>();

	public int hoveredIndex
	{
		get
		{
			return _hoveredIndex;
		}
		set
		{
			if (_hoveredIndex != value)
			{
				_hoveredIndex = value;
				onHoveredIndexChanged?.Invoke();
			}
		}
	}

	public StarEffect hoveredStar
	{
		get
		{
			if (_hoveredIndex != -1)
			{
				return items[_hoveredIndex].star;
			}
			return null;
		}
	}

	public int selectedIndex => listGroup.currentIndex;

	public StarEffect selectedStar
	{
		get
		{
			if (selectedIndex != -1)
			{
				return items[selectedIndex].star;
			}
			return null;
		}
	}

	private void Start()
	{
		if (DewBuildProfile.current.buildType != BuildType.DemoLite)
		{
			categoryGroup.onCurrentIndexChanged.AddListener(Refresh);
		}
	}

	private void OnEnable()
	{
		if (DewBuildProfile.current.buildType != BuildType.DemoLite && !(DewPlayer.local == null))
		{
			categoryGroup.currentIndex = 0;
			Refresh();
		}
	}

	private void Refresh(int arg0)
	{
		Refresh();
	}

	private void Refresh()
	{
		foreach (UI_Lobby_Constellations_StarItem item in items)
		{
			global::UnityEngine.Object.Destroy(item.gameObject);
		}
		items.Clear();
		HeroSkill hs = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType).GetComponent<HeroSkill>();
		characterSpecificTitle.text = DewLocalization.GetUIValue(DewPlayer.local.selectedHeroType + "_Name");
		IOrderedEnumerable<StarEffect> orderedEnumerable = DewResources.FindAllByTypeSubstring<StarEffect>("Se_Star_").OrderBy(delegate(StarEffect se)
		{
			if (se.skillType == null)
			{
				return -1;
			}
			for (int i = 0; i < hs.loadoutQ.Length; i++)
			{
				if (hs.loadoutQ[i].GetType() == se.skillType)
				{
					return i;
				}
			}
			for (int j = 0; j < hs.loadoutR.Length; j++)
			{
				if (hs.loadoutR[j].GetType() == se.skillType)
				{
					return 10 + j;
				}
			}
			for (int k = 0; k < hs.loadoutTrait.Length; k++)
			{
				if (hs.loadoutTrait[k].GetType() == se.skillType)
				{
					return 20 + k;
				}
			}
			return -1;
		}).ThenBy((StarEffect se) => se.requiredLevel)
			.ThenBy((StarEffect se) => se.requiredLevel);
		bool hasHeroItem = false;
		bool hasGlobalItem = false;
		foreach (StarEffect s in orderedEnumerable)
		{
			if ((!(s.heroType != null) || !(DewPlayer.local.selectedHeroType != s.heroType.Name)) && (categoryGroup.currentIndex != 0 || s.type == StarType.Destruction) && (categoryGroup.currentIndex != 1 || s.type == StarType.Life) && (categoryGroup.currentIndex != 2 || s.type == StarType.Imagination) && (categoryGroup.currentIndex != 3 || s.type == StarType.Flexible))
			{
				UI_Lobby_Constellations_StarItem newItem = global::UnityEngine.Object.Instantiate(itemPrefab, (s.heroType != null) ? characterSpecificGroup : globalGroup);
				if (s.heroType != null)
				{
					hasHeroItem = true;
				}
				else
				{
					hasGlobalItem = true;
				}
				newItem.Setup(s, items.Count);
				items.Add(newItem);
			}
		}
		seperatorObject.SetActive(hasHeroItem && hasGlobalItem);
		characterSpecificGroup.gameObject.SetActive(hasHeroItem);
		globalGroup.gameObject.SetActive(hasGlobalItem);
		characterSpecificTitle.gameObject.SetActive(hasHeroItem);
		globalTitle.gameObject.SetActive(hasGlobalItem);
		listGroup.currentIndex = -1;
	}

	public void SelectCategory(StarType type)
	{
		categoryGroup.currentIndex = (int)type;
	}

	public void SelectStar(StarEffect star)
	{
		categoryGroup.currentIndex = (int)star.type;
		listGroup.currentIndex = items.FindIndex((UI_Lobby_Constellations_StarItem i) => i.star == star);
	}
}
