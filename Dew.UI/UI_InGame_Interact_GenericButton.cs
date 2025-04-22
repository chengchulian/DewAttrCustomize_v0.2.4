using System;
using TMPro;

public class UI_InGame_Interact_GenericButton : UI_InGame_Interact_Base
{
	public TextMeshProUGUI displayText;

	public override Type GetSupportedType()
	{
		return typeof(IActivatable);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		displayText.text = DewLocalization.GetUIValue("InGame_Interact_ShrineActivate");
	}
}
