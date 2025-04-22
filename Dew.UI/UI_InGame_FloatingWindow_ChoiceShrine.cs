using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_InGame_FloatingWindow_ChoiceShrine : UI_InGame_FloatingWindow_Base
{
	public UI_InGame_FloatingWindow_ChoiceShrine_Item itemPrefab;

	public Transform itemsParent;

	public TextMeshProUGUI descriptionText;

	private List<UI_InGame_FloatingWindow_ChoiceShrine_Item> _items = new List<UI_InGame_FloatingWindow_ChoiceShrine_Item>();

	public override Type GetSupportedType()
	{
		return typeof(ChoiceShrine);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_ChoiceShrine_Open");
		descriptionText.text = DewLocalization.GetUIValue("InGame_ChoiceShrine_" + base.target.GetType().Name);
		UpdateItems();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(base.target is ChoiceShrine shrine) || shrine.IsNullOrInactive() || !shrine.isAvailable)
		{
			ManagerBase<FloatingWindowManager>.instance.ClearTarget();
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_ChoiceShrine_Close");
	}

	private void UpdateItems()
	{
		if (ManagerBase<FloatingWindowManager>.instance == null)
		{
			return;
		}
		MonoBehaviour target = ManagerBase<FloatingWindowManager>.instance.currentTarget;
		if (!(target == null) && target is ChoiceShrine { choices: var decs })
		{
			while (_items.Count > decs.Count)
			{
				global::UnityEngine.Object.Destroy(_items[0].gameObject);
				_items.RemoveAt(0);
			}
			while (_items.Count < decs.Count)
			{
				_items.Add(global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent));
			}
			for (int i = 0; i < decs.Count; i++)
			{
				_items[i].UpdateContent(decs[i], i);
			}
		}
	}

	public void ClickChoice(int index)
	{
		if (base.target is ChoiceShrine shrine && !shrine.IsNullOrInactive())
		{
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_ChoiceShrine_Choose");
			ManagerBase<FloatingWindowManager>.instance.ClearTarget();
			shrine.CmdChoose(index);
		}
	}
}
