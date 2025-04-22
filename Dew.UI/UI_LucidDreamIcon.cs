using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LucidDreamIcon : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public GameObject lockedObject;

	public Image icon;

	public Outline outline;

	public CanvasGroup highlightCg;

	public bool ignoreAchievement;

	private string _type;

	private LucidDream _dream;

	public void Setup(string type)
	{
		_type = type;
		_dream = DewResources.GetByShortTypeName<LucidDream>(type);
		icon.sprite = _dream.icon;
		ShowColor();
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowCollectableTooltip(new TooltipSettings
		{
			mode = TooltipPositionMode.RawValue,
			position = base.transform.position + Vector3.up * 30f
		}, _dream.GetType(), new CollectableTooltipSettings
		{
			alwaysUnlocked = ignoreAchievement
		});
	}

	public void ShowLocked()
	{
		lockedObject.SetActive(value: true);
		_dream.color.ToHSV(out var h, out var _, out var v);
		icon.color = Color.HSVToRGB(h, 0f, v * 0.5f);
		outline.effectColor = Color.HSVToRGB(h, 0f, 0.35f);
		highlightCg.alpha = 0f;
	}

	public void ShowColor()
	{
		lockedObject.SetActive(value: false);
		_dream.color.ToHSV(out var h, out var s, out var v);
		icon.color = Color.HSVToRGB(h, s, v);
		outline.effectColor = Color.HSVToRGB(h, 1f, 0.35f);
		highlightCg.alpha = 1f;
	}
}
