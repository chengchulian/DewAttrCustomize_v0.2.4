using System;
using TMPro;

public class UI_InGame_Interact_Merchant : UI_InGame_Interact_Base
{
	public TextMeshProUGUI nameText;

	public override Type GetSupportedType()
	{
		return typeof(PropEnt_Merchant_Base);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		nameText.text = DewLocalization.GetUIValue(base.interactable.GetType().Name + "_Name");
	}
}
