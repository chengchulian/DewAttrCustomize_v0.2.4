using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby_Constellations_Skills_ContextMenu : ContextMenu
{
	public UI_Lobby_Constellations_Skills_ContextMenu_Item itemPrefab;

	public Transform itemsParent;

	[NonSerialized]
	public HeroSkillLocation type;

	[NonSerialized]
	public SkillTrigger[] skills;

	[NonSerialized]
	public int currentSkill;

	public SafeAction onClose;

	private List<UI_Lobby_Constellations_Skills_ContextMenu_Item> _items = new List<UI_Lobby_Constellations_Skills_ContextMenu_Item>();

	private void Awake()
	{
		UI_Lobby_Constellations_Skills_ContextMenu_Item[] componentsInChildren = GetComponentsInChildren<UI_Lobby_Constellations_Skills_ContextMenu_Item>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			global::UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (skills != null)
		{
			while (_items.Count < skills.Length)
			{
				_items.Add(global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent));
			}
			while (_items.Count > skills.Length)
			{
				global::UnityEngine.Object.Destroy(_items[0].gameObject);
				_items.RemoveAt(0);
			}
			for (int i = 0; i < _items.Count; i++)
			{
				_items[i].Setup(i, skills[i], currentSkill == i);
			}
		}
	}

	public void ClickOnItem(int index)
	{
		SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout.SetSkill(type, index);
		SingletonBehaviour<UI_Lobby_Constellations>.instance.NotifyChange();
		base.gameObject.SetActive(value: false);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		onClose?.Invoke();
	}
}
