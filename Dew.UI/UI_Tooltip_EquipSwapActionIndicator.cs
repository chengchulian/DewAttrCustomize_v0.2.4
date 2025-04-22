using TMPro;
using UnityEngine;

public class UI_Tooltip_EquipSwapActionIndicator : UI_Tooltip_BaseObj
{
	public TextMeshProUGUI uiText;

	public override void OnSetup()
	{
		base.OnSetup();
		bool isEquip = base.currentObject == null || (Object)base.currentObject == ManagerBase<EditSkillManager>.instance.draggingObject;
		uiText.text = DewLocalization.GetUIValue(isEquip ? "InGame_Tooltip_Equip" : "InGame_Tooltip_Replace");
	}
}
