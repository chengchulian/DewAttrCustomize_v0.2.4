using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_NametagList : SingletonBehaviour<UI_NametagList>
{
	public UI_NametagList_Item itemPrefab;

	public RectTransform itemsParent;

	public TextMeshProUGUI progressText;

	public GameObject[] disableIfNoneExists;

	[NonSerialized]
	public List<UI_NametagList_Item> shownItems = new List<UI_NametagList_Item>();

	protected virtual void Start()
	{
		RefreshList();
	}

	public virtual void RefreshList()
	{
		if (SingletonBehaviour<UI_TooltipManager>.instance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
		foreach (UI_NametagList_Item shownItem in shownItems)
		{
			global::UnityEngine.Object.Destroy(shownItem.gameObject);
		}
		shownItems.Clear();
		List<(Nametag, DewProfile.CosmeticsData)> list = new List<(Nametag, DewProfile.CosmeticsData)>();
		foreach (KeyValuePair<string, DewProfile.CosmeticsData> e in DewSave.profile.nametags)
		{
			Nametag nametag = DewResources.GetByName<Nametag>(e.Key);
			if (!(nametag == null))
			{
				list.Add((nametag, e.Value));
			}
		}
		list.Sort(((Nametag, DewProfile.CosmeticsData) t0, (Nametag, DewProfile.CosmeticsData) t1) => string.Compare(t0.Item1.name, t1.Item1.name, StringComparison.Ordinal));
		int unlocked = 0;
		for (int i = 0; i < list.Count; i++)
		{
			(Nametag, DewProfile.CosmeticsData) t2 = list[i];
			UI_NametagList_Item newItem = global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent);
			newItem.Setup(t2.Item1.name);
			newItem.GetComponentInChildren<UI_Toggle>().index = i;
			shownItems.Add(newItem);
			if (t2.Item2.isUnlocked)
			{
				unlocked++;
			}
		}
		if (progressText != null)
		{
			progressText.text = $"{unlocked}/{list.Count}";
		}
		if (disableIfNoneExists != null)
		{
			disableIfNoneExists.SetActiveAll(list.Count > 0 && !DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth));
		}
	}

	private void OnDisable()
	{
		if (SingletonBehaviour<UI_TooltipManager>.instance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
	}
}
