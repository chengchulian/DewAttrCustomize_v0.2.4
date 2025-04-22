using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_AccList : SingletonBehaviour<UI_AccList>
{
	public UI_AccList_Item itemPrefab;

	public RectTransform itemsParent;

	public TextMeshProUGUI progressText;

	public GameObject[] disableIfNoneExists;

	public GameObject newObject;

	[NonSerialized]
	public List<UI_AccList_Item> shownItems = new List<UI_AccList_Item>();

	protected virtual void Start()
	{
		RefreshList();
	}

	public virtual void RefreshList()
	{
		foreach (UI_AccList_Item shownItem in shownItems)
		{
			global::UnityEngine.Object.Destroy(shownItem.gameObject);
		}
		shownItems.Clear();
		List<(Accessory, DewProfile.CosmeticsData)> list = new List<(Accessory, DewProfile.CosmeticsData)>();
		foreach (KeyValuePair<string, DewProfile.CosmeticsData> e in DewSave.profile.accessories)
		{
			Accessory accessory = DewResources.GetByName<Accessory>(e.Key);
			if (!(accessory == null))
			{
				list.Add((accessory, e.Value));
			}
		}
		list.Sort(((Accessory, DewProfile.CosmeticsData) t0, (Accessory, DewProfile.CosmeticsData) t1) => string.Compare(t0.Item1.name, t1.Item1.name, StringComparison.Ordinal));
		int unlocked = 0;
		for (int i = 0; i < list.Count; i++)
		{
			(Accessory, DewProfile.CosmeticsData) t2 = list[i];
			UI_AccList_Item newItem = global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent);
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
		UpdateNewStatus();
	}

	public void UpdateNewStatus()
	{
		if (newObject == null)
		{
			return;
		}
		newObject.SetActive(value: false);
		foreach (KeyValuePair<string, DewProfile.CosmeticsData> accessory in DewSave.profile.accessories)
		{
			if (accessory.Value.isNew)
			{
				newObject.SetActive(value: true);
			}
		}
	}
}
