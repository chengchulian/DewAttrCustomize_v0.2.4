using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame_FloatingWindow_Hatred : UI_InGame_FloatingWindow_Base
{
	public UI_InGame_FloatingWindow_Hatred_Item itemPrefab;

	public Transform itemsParent;

	private List<UI_InGame_FloatingWindow_Hatred_Item> _items = new List<UI_InGame_FloatingWindow_Hatred_Item>();

	public override Type GetSupportedType()
	{
		return typeof(Shrine_Hatred);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Chaos_Open");
		UpdateItems();
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Chaos_Close");
	}

	public void ClickChoice(int index)
	{
		if (base.target is Shrine_Hatred shrine && !shrine.IsNullOrInactive())
		{
			ManagerBase<FloatingWindowManager>.instance.ClearTarget();
			shrine.CmdChoose(index);
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Chaos_Choose");
		}
	}

	private void UpdateItems()
	{
		if (!(ManagerBase<FloatingWindowManager>.instance == null) && !(base.target == null) && base.target is Shrine_Hatred hatred)
		{
			while (_items.Count > hatred.choices.Count)
			{
				global::UnityEngine.Object.Destroy(_items[0].gameObject);
				_items.RemoveAt(0);
			}
			while (_items.Count < hatred.choices.Count)
			{
				_items.Add(global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent));
			}
			for (int i = 0; i < _items.Count; i++)
			{
				_items[i].UpdateContent(hatred.choices[i], i);
			}
		}
	}
}
