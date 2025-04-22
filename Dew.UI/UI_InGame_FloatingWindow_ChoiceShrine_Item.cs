using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_FloatingWindow_ChoiceShrine_Item : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPingableChoiceItem
{
	public Image gemIcon;

	public Image skillIcon;

	public Image rarityImage;

	public TextMeshProUGUI skillPlusText;

	public TextMeshProUGUI gemQualityText;

	private string _currentCachedType;

	private int _index;

	public Object cachedObject { get; private set; }

	public ChoiceShrineItem data { get; private set; }

	Object IPingableChoiceItem.shrine => ManagerBase<FloatingWindowManager>.instance.currentTarget as ChoiceShrine;

	int IPingableChoiceItem.choiceIndex => _index;

	public void UpdateContent(ChoiceShrineItem d, int index)
	{
		data = d;
		_index = index;
		if (d.typeName == _currentCachedType)
		{
			return;
		}
		_currentCachedType = d.typeName;
		Object obj = (cachedObject = DewResources.GetByShortTypeName(d.typeName));
		gemIcon.gameObject.SetActive(value: false);
		skillIcon.gameObject.SetActive(value: false);
		skillPlusText.gameObject.SetActive(value: false);
		gemQualityText.gameObject.SetActive(value: false);
		if (obj is Gem gem)
		{
			gemIcon.gameObject.SetActive(value: true);
			gemIcon.sprite = gem.icon;
			rarityImage.color = Dew.GetRarityColor(gem.rarity);
			gemQualityText.gameObject.SetActive(value: true);
			gemQualityText.text = $"{d.level:###,0}%";
		}
		else if (obj is SkillTrigger skill)
		{
			skillIcon.gameObject.SetActive(value: true);
			skillIcon.sprite = skill.configs[0].triggerIcon;
			rarityImage.color = Dew.GetRarityColor(skill.rarity);
			skillPlusText.gameObject.SetActive(value: true);
			if (d.level < 5)
			{
				skillPlusText.text = new string('+', d.level - 1);
			}
			else
			{
				skillPlusText.text = "+" + (d.level - 1).ToString("#,##0");
			}
		}
	}

	public void Click()
	{
		GetComponentInParent<UI_InGame_FloatingWindow_ChoiceShrine>().ClickChoice(_index);
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (cachedObject is SkillTrigger skill)
		{
			tooltip.ShowSkillTooltip(base.transform.position, skill, data.level);
		}
		else if (cachedObject is Gem gem)
		{
			tooltip.ShowGemTooltip(base.transform.position, gem, data.level);
		}
	}
}
