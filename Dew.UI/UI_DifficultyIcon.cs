using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_DifficultyIcon : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public string type;

	public Image icon;

	public NicerOutline outline;

	private DewDifficultySettings _settings;

	private void Start()
	{
		if (!string.IsNullOrEmpty(type))
		{
			Setup(type);
		}
	}

	public void Setup(string t)
	{
		type = t;
		_settings = DewResources.GetByName<DewDifficultySettings>(type);
		icon.sprite = _settings.icon;
		icon.transform.localScale = Vector3.one * _settings.iconScale;
		icon.color = _settings.difficultyColor.WithS(0.4f).WithV(1f);
		outline.effectColor = _settings.difficultyColor.WithV(0.375f);
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		Color c = _settings.difficultyColor;
		tooltip.ShowTitleDescTooltip(new TooltipSettings
		{
			mode = TooltipPositionMode.RawValue,
			position = base.transform.position + Vector3.up * 30f
		}, "<color=" + Dew.GetHex(c) + ">" + DewLocalization.GetUIValue("Difficulty_" + type + "_Name") + "</color>", "<color=" + Dew.GetHex(c.WithS(0.2f).WithV(1f)) + ">" + DewLocalization.GetUIValue("Difficulty_" + type + "_Description") + "</color>");
	}
}
