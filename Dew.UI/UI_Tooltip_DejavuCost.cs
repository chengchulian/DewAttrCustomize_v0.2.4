using UnityEngine;

public class UI_Tooltip_DejavuCost : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count != 2 || !(currentObjects[1] is string str) || str != "Dejavu" || DewSave.profile.itemStatistics[currentObjects[0].GetType().Name].wins <= 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		GetComponentInChildren<CostDisplay>().Setup(new Cost
		{
			stardust = ((!Dew.IsDejavuFree((Object)currentObjects[0])) ? Dew.GetDejavuCost((Object)currentObjects[0]) : 0)
		});
	}
}
