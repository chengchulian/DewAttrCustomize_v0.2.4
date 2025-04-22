using UnityEngine;

public class UI_Collectables_ObjectItemProvider_Gem : UI_Collectables_ObjectItemProvider
{
	public override DewProfile.UnlockData GetUnlockData()
	{
		return DewSave.profile.gems[targetObj.GetType().Name];
	}

	public override Color GetRarityColor()
	{
		return Dew.GetRarityColor(((Gem)targetObj).rarity);
	}

	public override Sprite GetIcon()
	{
		return ((Gem)targetObj).icon;
	}
}
