using UnityEngine;

public class UI_TooltipSection_Skill : UI_TooltipSection
{
	public int objIndex;

	public GameObject[] availableObjects;

	public GameObject[] emptyObjects;

	public bool hideIfFloatingSkill;

	protected override void OnEnable()
	{
		base.OnEnable();
		SkillTrigger skill = base.currentObjects[objIndex] as SkillTrigger;
		bool isAvailable = skill != null && (!hideIfFloatingSkill || ManagerBase<EditSkillManager>.instance.draggingObject != skill);
		if (base.currentObjects.Count == 3 && base.currentObjects[0] is DewGameResult result && base.currentObjects[1] is int playerIndex)
		{
			object obj = base.currentObjects[2];
			if (obj is HeroSkillLocation)
			{
				HeroSkillLocation skillType = (HeroSkillLocation)obj;
				if (result.players[playerIndex].skills.FindIndex((DewGameResult.SkillData s) => s.loc == skillType) >= 0)
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
