using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltip_WorldNode_Modifiers : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is int nodeIndex))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		WorldNodeData data = NetworkedManagerBase<ZoneManager>.instance.nodes[nodeIndex];
		List<string> visible = new List<string>();
		for (int i = 0; i < data.modifiers.Count; i++)
		{
			if (data.IsModifierVisible(i))
			{
				visible.Add(data.modifiers[i].type);
			}
		}
		if (visible.Count == 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		text.text = "";
		text.color = Color.white;
		for (int j = 0; j < visible.Count; j++)
		{
			Color color = DewResources.GetByShortTypeName<RoomModifierBase>(visible[j]).mainColor;
			string content = DewLocalization.GetUIValue(visible[j] + "_Tooltip");
			if (color.GetV() < 0.85f)
			{
				color = color.WithV(0.85f);
			}
			if (color.GetS() > 0.6f)
			{
				color = color.WithS(0.6f);
			}
			TextMeshProUGUI textMeshProUGUI = text;
			textMeshProUGUI.text = textMeshProUGUI.text + "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + content + "</color>";
			if (j < visible.Count - 1)
			{
				text.text += "\n";
			}
		}
	}
}
