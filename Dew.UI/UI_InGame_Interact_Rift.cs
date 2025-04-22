using System;
using TMPro;

public class UI_InGame_Interact_Rift : UI_InGame_Interact_Base
{
	public TextMeshProUGUI nameText;

	public TextMeshProUGUI actionText;

	public override Type GetSupportedType()
	{
		return typeof(Rift);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		UpdateInteractStatus();
		nameText.text = GetRiftNameText(base.interactable);
	}

	public static string GetRiftNameText(IInteractable interactable)
	{
		if (interactable is Rift_RoomExit)
		{
			if (NetworkedManagerBase<ZoneManager>.instance.currentNode.type == WorldNodeType.ExitBoss)
			{
				return DewLocalization.GetUIValue("Rift_ToSomewhereDeeper_Name");
			}
			return DewLocalization.GetUIValue("Rift_Name");
		}
		if (interactable is Rift_Sidetrack rift)
		{
			if (NetworkedManagerBase<ZoneManager>.instance.isSidetracking)
			{
				return DewLocalization.GetUIValue(NetworkedManagerBase<ZoneManager>.instance.currentZone.name + "_Name");
			}
			if (DewLocalization.TryGetUIValue(rift.name + "_Name", out var val))
			{
				return val;
			}
			return DewLocalization.GetUIValue("Rift_Otherworld_Name");
		}
		return DewLocalization.GetUIValue("Rift_Name");
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateInteractStatus();
	}

	private void UpdateInteractStatus()
	{
		Rift rift = (Rift)base.interactable;
		if (rift.isLocked)
		{
			actionText.text = DewLocalization.GetUIValue("InGame_Interact_Rift_Locked");
		}
		else if (rift is Rift_RoomExit && NetworkedManagerBase<ZoneManager>.instance.currentNode.type != WorldNodeType.ExitBoss)
		{
			actionText.text = DewLocalization.GetUIValue("InGame_Interact_Rift_OpenMap");
		}
		else if (rift is Rift_Sidetrack && NetworkedManagerBase<ZoneManager>.instance.isSidetracking)
		{
			actionText.text = DewLocalization.GetUIValue("InGame_Interact_Rift_GoBack");
		}
		else
		{
			actionText.text = DewLocalization.GetUIValue("InGame_Interact_Rift_Enter");
		}
	}
}
