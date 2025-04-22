using UnityEngine.UI;

public class UI_Tooltip_WorldNode_CanTravelObject : UI_Tooltip_BaseObj
{
	public LayoutElement layoutElement;

	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is int nodeIndex))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(InGameUIManager.instance.isWorldDisplayed == WorldDisplayStatus.Shown && NetworkedManagerBase<ZoneManager>.instance.IsNodeConnected(NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex, nodeIndex));
		if (layoutElement != null)
		{
			layoutElement.ignoreLayout = !base.gameObject.activeSelf;
		}
	}
}
