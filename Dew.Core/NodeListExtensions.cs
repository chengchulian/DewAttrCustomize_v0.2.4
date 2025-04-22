using System.Collections.Generic;
using DewInternal;

public static class NodeListExtensions
{
	public static string ToText(this List<LocaleNode> list, CurseStatusEffect curse)
	{
		DewLocalization.DescriptionSettings descriptionSettings = default(DewLocalization.DescriptionSettings);
		descriptionSettings.currentLevel = curse.currentStrength.GetValueIndex() + 1;
		DewLocalization.DescriptionSettings settings = descriptionSettings;
		return DewLocalization.ConvertDescriptionNodesToText(list, settings);
	}
}
