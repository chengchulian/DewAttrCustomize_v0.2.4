using System;
using System.Collections.Generic;
using DewInternal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_Constellations_StarDetails : MonoBehaviour
{
	public GameObject fxPurchase;

	public Image icon;

	public GameObject emptyObject;

	public GameObject nonEmptyObject;

	public TextMeshProUGUI starNameText;

	public TextMeshProUGUI heroNameText;

	public TextMeshProUGUI levelText;

	public TextMeshProUGUI descText;

	public TextMeshProUGUI loreText;

	public GameObject requiredSkillObject;

	public TextMeshProUGUI requiredSkillText;

	public Image requiredSkillImage;

	public UI_Lobby_Constellations_StarDetails_BuyButton buyButton;

	public TextMeshProUGUI buyButtonText;

	public GameObject insufficientObject;

	public TextMeshProUGUI insufficientReasonText;

	public GameObject maxLevelReachedObject;

	public CostDisplay costDisplay;

	public StarEffect star
	{
		get
		{
			UI_Lobby_Constellations_StarList list = SingletonBehaviour<UI_Lobby_Constellations_StarList>.instance;
			if (SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.hoveredStarSlot != null && SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.hoveredStarSlot.star != null)
			{
				return SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.hoveredStarSlot.star;
			}
			if (list.hoveredStar != null)
			{
				return list.hoveredStar;
			}
			return list.selectedStar;
		}
	}

	private void Start()
	{
		SingletonBehaviour<UI_Lobby_Constellations_StarList>.instance.onHoveredIndexChanged += new Action(UpdateDetails);
		SingletonBehaviour<UI_Lobby_Constellations_StarList>.instance.onSelectedIndexChanged += new Action(UpdateDetails);
		SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.onHoveredStarSlotChanged += new Action(UpdateDetails);
		buyButton.button.onClick.AddListener(OnClickBuy);
		buyButton.onIsHoveringChanged += new Action(UpdateDetails);
	}

	private void OnClickBuy()
	{
		if (insufficientObject.activeSelf || star == null)
		{
			return;
		}
		DewProfile.StarData d = DewSave.profile.newStars[star.GetType().Name];
		string template = DewLocalization.GetUIValue((d.level <= 0) ? "Constellations_IgniteStarConfirmation" : "Constellations_EmpowerStarConfirmation");
		int price = star.price[d.level];
		string starName = GetColoredPrefixedStarName(star);
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			owner = this,
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
			rawContent = string.Format(template, price, starName),
			defaultButton = DewMessageSettings.ButtonType.No,
			onClose = delegate(DewMessageSettings.ButtonType b)
			{
				if (b == DewMessageSettings.ButtonType.Yes)
				{
					DewSave.profile.spentStardust += price;
					DewSave.profile.stardust -= price;
					d.level++;
					DewSave.SaveProfile();
					DewEffect.PlayNew(fxPurchase);
					foreach (UI_Lobby_Constellations_StarItem item in SingletonBehaviour<UI_Lobby_Constellations_StarList>.instance.items)
					{
						item.Refresh();
					}
					UpdateDetails();
				}
			}
		});
	}

	public static string GetColoredPrefixedStarName(StarEffect star)
	{
		Color starColor = Dew.GetStarCategoryColor(star.type);
		starColor = starColor.WithS(starColor.GetS() * 0.5f).WithV(1f);
		string starName = DewLocalization.GetStarName(star.GetType().Name);
		if (star.heroType != null)
		{
			return "<color=" + Dew.GetHex(starColor) + ">" + starName + " (" + DewLocalization.GetUIValue(star.heroType.Name + "_Name") + ")</color>";
		}
		return "<color=" + Dew.GetHex(starColor) + ">" + starName + "</color>";
	}

	private void UpdateDetails()
	{
		if (star == null)
		{
			emptyObject.SetActive(value: true);
			nonEmptyObject.SetActive(value: false);
			return;
		}
		emptyObject.SetActive(value: false);
		nonEmptyObject.SetActive(value: true);
		Color color = Dew.GetStarCategoryColor(star.type);
		if (star.starIcon != null)
		{
			icon.sprite = star.starIcon;
		}
		icon.color = Color.Lerp(color, Color.grey, 0.5f);
		Color starColor = Dew.GetStarCategoryColor(star.type);
		starColor = starColor.WithS(starColor.GetS() * 0.5f).WithV(1f);
		string starName = DewLocalization.GetStarName(star.GetType().Name);
		starNameText.text = "<color=" + Dew.GetHex(starColor) + ">" + starName + "</color>";
		levelText.text = $"{DewSave.profile.newStars[star.GetType().Name].level}/{star.maxStarLevel}";
		levelText.color = starColor.WithA(0.5f);
		List<LocaleNode> nodes = DewLocalization.GetStarDescription(star.GetType().Name);
		int[] levels = new int[star.maxStarLevel];
		for (int i = 0; i < star.maxStarLevel; i++)
		{
			levels[i] = i + 1;
		}
		if (buyButton.isHovering && DewSave.profile.newStars[star.GetType().Name].level != 0)
		{
			descText.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
			{
				previousLevel = Mathf.Clamp(DewSave.profile.newStars[star.GetType().Name].level, 1, star.maxStarLevel),
				currentLevel = Mathf.Clamp(DewSave.profile.newStars[star.GetType().Name].level + 1, 1, star.maxStarLevel),
				showLevelScaling = true
			});
		}
		else
		{
			descText.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
			{
				currentLevel = Mathf.Clamp(DewSave.profile.newStars[star.GetType().Name].level, 1, star.maxStarLevel),
				levels = levels
			});
		}
		if (star.heroType != null)
		{
			heroNameText.gameObject.SetActive(value: true);
			DewResources.GetByShortTypeName<Hero>(star.heroType.Name);
			heroNameText.text = "(" + DewLocalization.GetUIValue(star.heroType.Name + "_Name") + ")";
			heroNameText.color = starColor.WithA(0.5f);
		}
		else
		{
			heroNameText.gameObject.SetActive(value: false);
		}
		if (star.skillType != null)
		{
			SkillTrigger s = DewResources.GetByShortTypeName<SkillTrigger>(star.skillType.Name);
			requiredSkillObject.SetActive(value: true);
			requiredSkillText.text = DewLocalization.GetSkillName(DewLocalization.GetSkillKey(star.skillType.Name), 0);
			requiredSkillText.color = Dew.GetRarityColor(s.rarity);
			requiredSkillImage.sprite = s.configs[0].triggerIcon;
		}
		else
		{
			requiredSkillObject.SetActive(value: false);
		}
		bool insufficientRequirement = ((!(star.heroType == null)) ? (DewSave.profile.heroMasteries[star.heroType.Name].currentLevel < star.requiredLevel) : (DewSave.profile.totalMasteryLevel < star.requiredLevel));
		int currLvl = DewSave.profile.newStars[star.GetType().Name].level;
		if (currLvl >= star.maxStarLevel)
		{
			maxLevelReachedObject.SetActive(value: true);
			buyButton.gameObject.SetActive(value: false);
			insufficientObject.SetActive(value: false);
			costDisplay.gameObject.SetActive(value: false);
		}
		else
		{
			int cost = star.price[currLvl];
			maxLevelReachedObject.SetActive(value: false);
			costDisplay.gameObject.SetActive(value: true);
			costDisplay.Setup(new Cost
			{
				stardust = cost
			});
			if (insufficientRequirement)
			{
				insufficientObject.SetActive(value: true);
				buyButton.gameObject.SetActive(value: false);
				if (star.heroType == null)
				{
					insufficientReasonText.text = string.Format(DewLocalization.GetUIValue("Constellations_TotalMasteryRequirement"), star.requiredLevel);
				}
				else
				{
					insufficientReasonText.text = string.Format(DewLocalization.GetUIValue("Constellations_TravelerMasteryRequirement"), DewLocalization.GetUIValue(star.heroType.Name + "_Name"), star.requiredLevel);
				}
			}
			else
			{
				insufficientObject.SetActive(value: false);
				buyButton.gameObject.SetActive(value: true);
				buyButton.button.interactable = DewSave.profile.stardust >= cost;
				buyButtonText.text = DewLocalization.GetUIValue((currLvl <= 0) ? "Constellations_Ignite" : "Constellations_Empower");
			}
		}
		loreText.text = DewLocalization.GetStarLore(star.GetType().Name);
	}
}
