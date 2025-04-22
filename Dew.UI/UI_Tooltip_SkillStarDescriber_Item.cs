using System.Collections.Generic;
using DewInternal;
using TMPro;
using UnityEngine;

public class UI_Tooltip_SkillStarDescriber_Item : MonoBehaviour
{
	public void Setup(string type, Dictionary<string, string> capturedFields)
	{
		string desc = DewLocalization.ConvertDescriptionNodesToText(DewLocalization.GetStarDescription(type), new DewLocalization.DescriptionSettings
		{
			capturedFields = capturedFields,
			isSkillStar = true
		});
		GetComponent<TextMeshProUGUI>().text = FormatWithStarName(type, desc);
	}

	public void Setup(string type, int level, Entity entity)
	{
		List<LocaleNode> nodes = DewLocalization.GetStarDescription(type);
		DewLocalization.DescriptionSettings descriptionSettings = default(DewLocalization.DescriptionSettings);
		descriptionSettings.currentLevel = level;
		descriptionSettings.contextEntity = entity;
		descriptionSettings.isSkillStar = true;
		DewLocalization.DescriptionSettings settings = descriptionSettings;
		GetComponent<TextMeshProUGUI>().text = FormatWithStarName(type, DewLocalization.ConvertDescriptionNodesToText(nodes, settings));
	}

	private string FormatWithStarName(string starType, string desc)
	{
		return "<color=yellow><b>[" + DewLocalization.GetStarName(starType) + "]</b></color> " + desc;
	}
}
