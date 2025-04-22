using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Collectables_MainView : View
{
	private readonly HeroSkillLocation[] SkillTypeOrder = new HeroSkillLocation[3]
	{
		HeroSkillLocation.Q,
		HeroSkillLocation.R,
		HeroSkillLocation.Identity
	};

	public UI_Collectables_ObjectDetails detailsDisplay;

	public UI_Collectables_ObjectItem skillPrefab;

	public UI_Collectables_ObjectItem gemPrefab;

	public UI_Collectables_ObjectItem artifactPrefab;

	public ScrollRect scrollRect;

	public UI_ToggleGroup categoryGroup;

	public UI_ToggleGroup[] groups;

	public TextMeshProUGUI[] progressTexts;

	public Transform normalSkillsParent;

	public GameObject heroSkillTemplate;

	public GameObject objectListObject;

	public GameObject emotesObject;

	public GameObject accessoriesObject;

	public GameObject nametagsObject;

	public GameObject journalObject;

	private Dictionary<Hero, Transform> _heroSkillParents = new Dictionary<Hero, Transform>();

	private List<global::UnityEngine.Object> _shownObjects = new List<global::UnityEngine.Object>();

	protected override void Start()
	{
		base.Start();
		if (!Application.IsPlaying(this))
		{
			return;
		}
		emotesObject.SetActive(value: true);
		accessoriesObject.SetActive(value: true);
		nametagsObject.SetActive(value: true);
		journalObject.SetActive(value: true);
		Dew.CallDelayed(delegate
		{
			emotesObject.SetActive(value: false);
			accessoriesObject.SetActive(value: false);
			nametagsObject.SetActive(value: false);
			journalObject.SetActive(value: false);
		});
		categoryGroup.currentIndex = 0;
		categoryGroup.onCurrentIndexChanged.AddListener(delegate
		{
			Refresh();
		});
		UI_ToggleGroup[] array = groups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].onCurrentIndexChanged.AddListener(ShowObjectListDetails);
		}
		heroSkillTemplate.gameObject.SetActive(value: false);
		for (int j = 0; j < progressTexts.Length; j++)
		{
			TextMeshProUGUI t = progressTexts[j];
			int total = 0;
			int unlocked = 0;
			switch (j)
			{
			case 0:
				foreach (Type type3 in Dew.allSkills)
				{
					if (!type3.Name.Contains("_M_") && Dew.IsSkillIncludedInGame(type3.Name))
					{
						total++;
						if (DewSave.profile.skills[type3.Name].status == UnlockStatus.Complete)
						{
							unlocked++;
						}
					}
				}
				break;
			case 1:
				foreach (Type type2 in Dew.allGems)
				{
					if (Dew.IsGemIncludedInGame(type2.Name))
					{
						total++;
						if (DewSave.profile.gems[type2.Name].status == UnlockStatus.Complete)
						{
							unlocked++;
						}
					}
				}
				break;
			case 2:
				foreach (Type type in Dew.allArtifacts)
				{
					if (Dew.IsArtifactIncludedInGame(type.Name))
					{
						total++;
						if (DewSave.profile.artifacts[type.Name].status == UnlockStatus.Complete)
						{
							unlocked++;
						}
					}
				}
				break;
			}
			t.text = $"{unlocked:#,##0}/{total:#,##0}";
		}
	}

	protected override void OnShow()
	{
		base.OnShow();
		Refresh();
	}

	private void Refresh()
	{
		if (categoryGroup.currentIndex < 3)
		{
			RefreshObjectList();
			return;
		}
		objectListObject.SetActive(value: false);
		emotesObject.SetActive(categoryGroup.currentIndex == 3);
		accessoriesObject.SetActive(categoryGroup.currentIndex == 4);
		nametagsObject.SetActive(categoryGroup.currentIndex == 5);
		journalObject.SetActive(categoryGroup.currentIndex == 6);
	}

	private void RefreshObjectList()
	{
		objectListObject.SetActive(value: true);
		emotesObject.SetActive(value: false);
		accessoriesObject.SetActive(value: false);
		nametagsObject.SetActive(value: false);
		journalObject.SetActive(value: false);
		_shownObjects.Clear();
		for (int i = 0; i < groups.Length; i++)
		{
			groups[i].gameObject.SetActive(categoryGroup.currentIndex == i);
		}
		switch (categoryGroup.currentIndex)
		{
		case 0:
			foreach (Type s in Dew.allSkills)
			{
				if (Dew.IsSkillIncludedInGame(s.Name) && !s.Name.StartsWith("St_M_"))
				{
					SkillTrigger skill = DewResources.GetByType<SkillTrigger>(s);
					_shownObjects.Add(skill);
				}
			}
			break;
		case 1:
			foreach (Type g2 in Dew.allGems)
			{
				if (Dew.IsGemIncludedInGame(g2.Name))
				{
					Gem gem = DewResources.GetByType<Gem>(g2);
					_shownObjects.Add(gem);
				}
			}
			break;
		case 2:
			foreach (Type g in Dew.allArtifacts)
			{
				if (Dew.IsArtifactIncludedInGame(g.Name))
				{
					Artifact a = DewResources.GetByType<Artifact>(g);
					_shownObjects.Add(a);
				}
			}
			break;
		}
		Dictionary<SkillTrigger, (HeroSkillLocation, int, Hero)> characterSkills = new Dictionary<SkillTrigger, (HeroSkillLocation, int, Hero)>();
		for (int j = 0; j < Dew.allHeroes.Count; j++)
		{
			if (!Dew.IsHeroIncludedInGame(Dew.allHeroes[j].Name))
			{
				continue;
			}
			Hero h2 = DewResources.GetByType<Hero>(Dew.allHeroes[j]);
			HeroSkill hs = h2.GetComponent<HeroSkill>();
			for (int k = 0; k < SkillTypeOrder.Length; k++)
			{
				SkillTrigger[] skills = hs.GetLoadoutSkills(SkillTypeOrder[k]);
				for (int l = 0; l < skills.Length; l++)
				{
					SkillTrigger s2 = skills[l];
					if (!characterSkills.ContainsKey(s2))
					{
						characterSkills.Add(s2, (SkillTypeOrder[k], j * 100 + 10 * k + l, h2));
					}
				}
			}
		}
		Comparer<global::UnityEngine.Object> sorter = Comparer<global::UnityEngine.Object>.Create(delegate(global::UnityEngine.Object x, global::UnityEngine.Object y)
		{
			if (x is SkillTrigger skillTrigger && y is SkillTrigger skillTrigger2)
			{
				if (skillTrigger.rarity == skillTrigger2.rarity && (skillTrigger.rarity == Rarity.Character || skillTrigger.rarity == Rarity.Identity) && characterSkills.TryGetValue(skillTrigger, out var value) && characterSkills.TryGetValue(skillTrigger2, out var value2))
				{
					int item = value.Item2;
					int item2 = value2.Item2;
					if (item < item2)
					{
						return -1;
					}
					if (item > item2)
					{
						return 1;
					}
				}
				if ((int)skillTrigger.rarity < (int)skillTrigger2.rarity)
				{
					return -1;
				}
				if ((int)skillTrigger.rarity > (int)skillTrigger2.rarity)
				{
					return 1;
				}
				return string.Compare(skillTrigger.GetType().Name, skillTrigger2.GetType().Name, StringComparison.Ordinal);
			}
			if (x is Gem gem2 && y is Gem gem3)
			{
				if ((int)gem2.rarity < (int)gem3.rarity)
				{
					return -1;
				}
				if ((int)gem2.rarity > (int)gem3.rarity)
				{
					return 1;
				}
				return string.Compare(gem2.GetType().Name, gem3.GetType().Name, StringComparison.Ordinal);
			}
			return 0;
		});
		_shownObjects.Sort(sorter);
		UI_Collectables_ObjectItem[] componentsInChildren = GetComponentsInChildren<UI_Collectables_ObjectItem>(includeInactive: true);
		for (int m = 0; m < componentsInChildren.Length; m++)
		{
			global::UnityEngine.Object.Destroy(componentsInChildren[m].gameObject);
		}
		UI_ToggleGroup itemParentGroup = groups[categoryGroup.currentIndex];
		for (int n = 0; n < _shownObjects.Count; n++)
		{
			global::UnityEngine.Object o2 = _shownObjects[n];
			if (TryGetItemParent(o2, out var parent2))
			{
				UI_Collectables_ObjectItem prefab = null;
				if (o2 is SkillTrigger)
				{
					prefab = skillPrefab;
				}
				else if (o2 is Gem)
				{
					prefab = gemPrefab;
				}
				else if (o2 is Artifact)
				{
					prefab = artifactPrefab;
				}
				global::UnityEngine.Object.Instantiate(prefab, parent2).Setup(o2, n);
			}
		}
		itemParentGroup.currentIndex = -1;
		ClearDetails();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		scrollRect.normalizedPosition = Vector2.one;
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			StartCoroutine(Routine());
		}
		Transform GetHeroParent(Hero h)
		{
			if (!_heroSkillParents.ContainsKey(h))
			{
				GameObject newParent = global::UnityEngine.Object.Instantiate(heroSkillTemplate, groups[0].transform);
				newParent.GetComponentInChildren<TextMeshProUGUI>().text = DewLocalization.GetUIValue("Hero_" + h.GetType().Name.Replace("Hero_", "") + "_Name");
				_heroSkillParents[h] = newParent.GetComponentInChildren<GridLayoutGroup>().transform;
			}
			return _heroSkillParents[h];
		}
		static IEnumerator Routine()
		{
			yield return null;
			ManagerBase<GlobalUIManager>.instance.SetFocus(null);
		}
		bool TryGetItemParent(global::UnityEngine.Object o, out Transform parent)
		{
			if (o is SkillTrigger skill2)
			{
				if (characterSkills.TryGetValue(skill2, out var v))
				{
					if (!DewSave.profile.heroes[v.Item3.GetType().Name].isAvailableInGame)
					{
						parent = null;
						return false;
					}
					parent = GetHeroParent(v.Item3);
					return true;
				}
				parent = normalSkillsParent;
				return true;
			}
			parent = itemParentGroup.transform;
			return true;
		}
	}

	private void ShowObjectListDetails(int index)
	{
		if (index < 0 || index >= _shownObjects.Count)
		{
			detailsDisplay.Clear();
			return;
		}
		string objName = _shownObjects[index].GetType().Name;
		if ((_shownObjects[index] is SkillTrigger && DewSave.profile.skills[objName].status != UnlockStatus.Complete) || (_shownObjects[index] is Gem && DewSave.profile.gems[objName].status != UnlockStatus.Complete) || (_shownObjects[index] is Artifact && DewSave.profile.artifacts[objName].status != UnlockStatus.Complete))
		{
			detailsDisplay.Clear();
		}
		else
		{
			detailsDisplay.Show(_shownObjects[index]);
		}
	}

	private void ClearDetails()
	{
		detailsDisplay.Clear();
	}
}
