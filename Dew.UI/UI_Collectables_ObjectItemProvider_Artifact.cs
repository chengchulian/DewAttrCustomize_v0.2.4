using UnityEngine;

public class UI_Collectables_ObjectItemProvider_Artifact : UI_Collectables_ObjectItemProvider
{
	public override DewProfile.UnlockData GetUnlockData()
	{
		return DewSave.profile.artifacts[targetObj.GetType().Name];
	}

	public override Color GetRarityColor()
	{
		return ((Artifact)targetObj).mainColor;
	}

	public override Sprite GetIcon()
	{
		return ((Artifact)targetObj).icon;
	}
}
