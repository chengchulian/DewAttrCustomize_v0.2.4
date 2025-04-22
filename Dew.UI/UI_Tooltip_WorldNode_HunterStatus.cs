public class UI_Tooltip_WorldNode_HunterStatus : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is int nodeIndex) || (NetworkedManagerBase<ZoneManager>.instance.hunterStatuses[nodeIndex] == HunterStatus.None && NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex != nodeIndex))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		if (NetworkedManagerBase<ZoneManager>.instance.hunterStatuses[nodeIndex] == HunterStatus.None && NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex == nodeIndex)
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_HunterStatus_HunterStartLocation");
		}
		else if (NetworkedManagerBase<ZoneManager>.instance.hunterStatuses[nodeIndex] == HunterStatus.AboutToBeTaken)
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_HunterStatus_AboutToBeTaken");
		}
		else
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_WorldNode_HunterStatus_Taken");
		}
	}
}
