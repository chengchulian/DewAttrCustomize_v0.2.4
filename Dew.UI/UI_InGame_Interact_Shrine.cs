using System;
using TMPro;

public class UI_InGame_Interact_Shrine : UI_InGame_Interact_Base
{
	public TextMeshProUGUI nameText;

	public ActionDisplay actionDisplay;

	public CostDisplay costDisplay;

	public override Type GetSupportedType()
	{
		return typeof(Shrine);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (base.interactable is IShrineCustomName customName)
		{
			nameText.text = customName.GetRawName();
		}
		else
		{
			nameText.text = DewLocalization.GetUIValue(base.interactable.GetType().Name + "_Name");
		}
		UpdateStatus();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		Shrine obj = (Shrine)base.interactable;
		Entity activator = DewPlayer.local.controllingEntity;
		Cost cost = obj.GetCost(activator);
		if (cost.gold == 0 && cost.stardust == 0 && cost.dreamDust == 0 && cost.healthPercentage == 0)
		{
			costDisplay.gameObject.SetActive(value: false);
		}
		else
		{
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.Setup(cost);
		}
		string customAction = ((base.interactable is IShrineCustomAction ca) ? ca.GetRawAction() : null);
		if (obj.isLocked)
		{
			actionDisplay.text.text = customAction ?? DewLocalization.GetUIValue("InGame_Interact_ShrineActivateLocked");
			actionDisplay.isDisabled = true;
			return;
		}
		AffordType afford = cost.CanAfford(activator);
		switch (afford)
		{
		case AffordType.Yes:
			actionDisplay.text.text = customAction ?? DewLocalization.GetUIValue("InGame_Interact_ShrineActivate");
			actionDisplay.isDisabled = false;
			break;
		case AffordType.NoGold:
		case AffordType.NoDreamDust:
		case AffordType.NoStardust:
		case AffordType.NoHealth:
			actionDisplay.text.text = customAction ?? DewLocalization.GetUIValue("InGame_Interact_ShrineActivate" + afford);
			actionDisplay.isDisabled = true;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
