using UnityEngine;

public class UI_TooltipSection_Gem : UI_TooltipSection
{
	public int objIndex;

	public GameObject[] availableObjects;

	public GameObject[] emptyObjects;

	public bool hideIfFloatingGem;

	protected override void OnEnable()
	{
		base.OnEnable();
		Gem gem = base.currentObjects[objIndex] as Gem;
		bool isAvailable = gem != null && (!hideIfFloatingGem || ManagerBase<EditSkillManager>.instance.draggingObject != gem);
		if (base.currentObjects.Count == 3 && base.currentObjects[0] is DewGameResult result && base.currentObjects[1] is int playerIndex)
		{
			object obj = base.currentObjects[2];
			if (obj is GemLocation)
			{
				GemLocation gemLocation = (GemLocation)obj;
				if (result.players[playerIndex].gems.FindIndex((DewGameResult.GemData g) => g.location == gemLocation) >= 0)
				{
					isAvailable = true;
				}
			}
		}
		GameObject[] array = availableObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(isAvailable);
		}
		array = emptyObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!isAvailable);
		}
	}
}
