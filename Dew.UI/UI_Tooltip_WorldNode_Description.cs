using TMPro;

public class UI_Tooltip_WorldNode_Description : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is int nodeIndex))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		WorldNodeData node = NetworkedManagerBase<ZoneManager>.instance.nodes[nodeIndex];
		base.gameObject.SetActive(value: true);
		if (nodeIndex == NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex)
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_YouAreHere");
		}
		else if (node.status == WorldNodeStatus.Unexplored)
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_Unexplored");
		}
		else if (node.type.IsNormalNode())
		{
			if (node.status == WorldNodeStatus.HasVisited)
			{
				text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_Visited");
			}
			else
			{
				text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_Unvisited");
			}
		}
		else
		{
			if (node.type == WorldNodeType.Merchant && NetworkedManagerBase<ZoneManager>.instance.hunterStatuses[nodeIndex] != 0)
			{
				text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_MerchantDisabled");
			}
			else
			{
				text.text = DewLocalization.GetUIValue($"InGame_Tooltip_WorldNode_{node.type}");
			}
			if (node.status == WorldNodeStatus.HasVisited)
			{
				TextMeshProUGUI textMeshProUGUI = text;
				textMeshProUGUI.text = textMeshProUGUI.text + " <color=#7594A6>" + DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_AlreadyVisited") + "</color>";
			}
		}
		if (InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.Shown && NetworkedManagerBase<ZoneManager>.instance.GetNodeDistance(NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex, nodeIndex) > 1)
		{
			TextMeshProUGUI textMeshProUGUI2 = text;
			textMeshProUGUI2.text = textMeshProUGUI2.text + " <color=#caa>" + DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_TooFarToTravel") + "</color>";
		}
	}
}
