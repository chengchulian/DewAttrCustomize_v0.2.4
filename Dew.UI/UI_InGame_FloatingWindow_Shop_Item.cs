using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_FloatingWindow_Shop_Item : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPingableShopItem
{
	public CostDisplay costDisplay;

	public TextMeshProUGUI quantityText;

	public Image gemIcon;

	public Image skillIcon;

	public Image treasureIcon;

	public Image rarityImage;

	public GameObject upgradeableObject;

	public TextMeshProUGUI skillPlusText;

	public TextMeshProUGUI gemQualityText;

	private Button _button;

	private string _currentCachedType;

	private int _index;

	public Object cachedObject { get; private set; }

	public MerchandiseData data { get; private set; }

	Object IPingableShopItem.shop => ManagerBase<FloatingWindowManager>.instance.currentTarget;

	int IPingableShopItem.merchandiseIndex => _index;

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	public void UpdateContent(MerchandiseData d, int index)
	{
		data = d;
		_index = index;
		if (d.type == MerchandiseType.Empty)
		{
			_currentCachedType = null;
			upgradeableObject.SetActive(value: false);
			gemIcon.gameObject.SetActive(value: false);
			skillIcon.gameObject.SetActive(value: false);
			treasureIcon.gameObject.SetActive(value: false);
			skillPlusText.gameObject.SetActive(value: false);
			gemQualityText.gameObject.SetActive(value: false);
			costDisplay.gameObject.SetActive(value: false);
			rarityImage.color = Color.clear;
			cachedObject = null;
			quantityText.text = null;
			_button.interactable = false;
			return;
		}
		if (d.itemName != _currentCachedType)
		{
			_currentCachedType = d.itemName;
			Object obj = (cachedObject = ((d.type == MerchandiseType.Souvenir) ? DewResources.GetByName(d.itemName) : DewResources.GetByShortTypeName(d.itemName)));
			gemIcon.gameObject.SetActive(value: false);
			skillIcon.gameObject.SetActive(value: false);
			treasureIcon.gameObject.SetActive(value: false);
			skillPlusText.gameObject.SetActive(value: false);
			gemQualityText.gameObject.SetActive(value: false);
			if (obj is Gem gem)
			{
				gemIcon.gameObject.SetActive(value: true);
				gemIcon.sprite = gem.icon;
				rarityImage.color = Dew.GetRarityColor(gem.rarity);
				gemQualityText.gameObject.SetActive(value: true);
			}
			else if (obj is SkillTrigger skill)
			{
				skillIcon.gameObject.SetActive(value: true);
				skillIcon.sprite = skill.configs[0].triggerIcon;
				rarityImage.color = Dew.GetRarityColor(skill.rarity);
				skillPlusText.gameObject.SetActive(value: true);
			}
			else if (obj is Treasure treasure)
			{
				rarityImage.color = new Color(0.4f, 1f, 0.4f);
				treasureIcon.sprite = treasure.icon;
				treasureIcon.gameObject.SetActive(value: true);
			}
		}
		if (_currentCachedType.StartsWith("Gem_"))
		{
			gemQualityText.text = $"{d.level:###,0}%";
		}
		else if (_currentCachedType.StartsWith("St_"))
		{
			if (d.level < 5)
			{
				skillPlusText.text = new string('+', d.level - 1);
			}
			else
			{
				skillPlusText.text = "+" + (d.level - 1).ToString("#,##0");
			}
		}
		Cost price = (Cost)(d.price * DewPlayer.local.buyPriceMultiplier);
		costDisplay.gameObject.SetActive(value: true);
		costDisplay.Setup(price);
		quantityText.text = d.count.ToString("#,##0");
		_button.interactable = d.count > 0;
		if (d.type == MerchandiseType.Gem)
		{
			upgradeableObject.SetActive(d.count > 0 && DewPlayer.local.hero.Skill.gems.Any((KeyValuePair<GemLocation, Gem> p) => p.Value.GetType().Name.Equals(d.itemName)));
		}
		else
		{
			upgradeableObject.SetActive(value: false);
		}
	}

	public void Click()
	{
		GetComponentInParent<UI_InGame_FloatingWindow_Shop>().ClickMerchandise(_index);
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
		else if (cachedObject is Treasure trea)
		{
			tooltip.ShowTreasureTooltip(base.transform.position, trea, data.price.gold, data.customData);
		}
		else if (cachedObject is Accessory acc)
		{
			tooltip.ShowSouvenirTooltip(base.transform.position, acc.name);
		}
		else
		{
			tooltip.ShowRawTextTooltip(base.transform.position, DewLocalization.GetUIValue("Generic_Empty"));
		}
	}
}
