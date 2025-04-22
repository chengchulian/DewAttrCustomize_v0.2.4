using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_Lobby_Constellations_HeroConstellation_StarSlot : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IShowTooltip
{
	public enum AvailabilityType
	{
		Unlocked,
		Available,
		AdjacentRequirement,
		InsufficientStardust,
		RequiredMasteryLevel
	}

	public Transform tooltipPivot;

	public GameObject lockedObject;

	public GameObject fxUnlock;

	public GameObject emptyObject;

	public Image emptyIconImage;

	public Image[] emptyIconColoredImages;

	public Sprite[] categoryIconSprites;

	public GameObject nonEmptyObject;

	public Image[] tintedImages;

	public GameObject lockIconObject;

	public CostDisplay costDisplay;

	public TextMeshProUGUI levelReqText;

	public GameObject incompatibleObject;

	[NonSerialized]
	public List<UI_Lobby_Constellations_HeroConstellation_StarSlot> adjacentSlots = new List<UI_Lobby_Constellations_HeroConstellation_StarSlot>();

	private UI_StarIcon _starIcon;

	private Vector3 _ogScale;

	public StarType type { get; private set; }

	public int index { get; private set; } = -1;

	public bool isLocked { get; private set; }

	public AvailabilityType availability { get; private set; }

	public StarEffect star { get; private set; }

	private void Start()
	{
		_ogScale = base.transform.localScale;
		SingletonBehaviour<UI_Lobby_Constellations>.instance.onIsDraggingChanged += new Action(OnIsDraggingChanged);
		SingletonBehaviour<UI_Lobby_Constellations>.instance.onLoadoutChanged += new Action(Refresh);
		Refresh();
		OnIsDraggingChanged();
	}

	private void OnDestroy()
	{
		if (SingletonBehaviour<UI_Lobby_Constellations>.instance != null)
		{
			SingletonBehaviour<UI_Lobby_Constellations>.instance.onLoadoutChanged -= new Action(Refresh);
			SingletonBehaviour<UI_Lobby_Constellations>.instance.onIsDraggingChanged -= new Action(OnIsDraggingChanged);
		}
	}

	private void OnIsDraggingChanged()
	{
		UI_Lobby_Constellations c = SingletonBehaviour<UI_Lobby_Constellations>.instance;
		_starIcon.GetComponent<CanvasGroup>().alpha = ((c.isDragging && c.isDraggingFromSlot && c.draggingSlotType == type && c.draggingSlotIndex == index) ? 0.1f : 1f);
		Vector3 targetScale = ((c.isDragging && !isLocked) ? (_ogScale * 1.25f) : _ogScale);
		base.transform.DOKill();
		base.transform.DOScale(targetScale, 0.1f);
	}

	public void Setup(int i, StarType t)
	{
		_starIcon = GetComponentInChildren<UI_StarIcon>(includeInactive: true);
		type = t;
		index = i;
		Color c = Dew.GetStarCategoryColor(t);
		Image[] array = tintedImages;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].color *= c;
		}
		Refresh();
	}

	private void Refresh()
	{
		if (SingletonBehaviour<UI_Lobby_Constellations>.instance == null || SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout == null)
		{
			return;
		}
		HeroLoadoutData l = SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout;
		string hName = DewPlayer.local.selectedHeroType;
		Hero h = DewResources.GetByShortTypeName<Hero>(hName);
		List<LoadoutStarItem> list = l.GetStarList(type);
		Color c = Dew.GetStarCategoryColor(type);
		HeroConstellationSettings s = h.GetConstellationSettings(type);
		bool isUnlocked = index < s.defaultCount || DewSave.profile.heroUnlockedStarSlots[hName].Get(type).Contains(index);
		lockedObject.SetActive(!isUnlocked);
		isLocked = !isUnlocked;
		if (!isUnlocked)
		{
			int indexAmongLocked = index - s.defaultCount;
			costDisplay.gameObject.SetActive(value: false);
			lockIconObject.gameObject.SetActive(value: true);
			levelReqText.text = Dew.GetRequiredMasteryLevelForStarSlotUnlock(indexAmongLocked).ToString();
			costDisplay.Setup(new Cost
			{
				stardust = Dew.GetRequiredStardustForStarSlotUnlock(indexAmongLocked)
			});
		}
		if (isLocked)
		{
			int indexAmongLocked2 = index - s.defaultCount;
			DewProfile.HeroMasteryData m = DewSave.profile.heroMasteries[DewPlayer.local.selectedHeroType];
			if (adjacentSlots.All((UI_Lobby_Constellations_HeroConstellation_StarSlot slot) => slot.isLocked))
			{
				availability = AvailabilityType.AdjacentRequirement;
			}
			else if (m.currentLevel < Dew.GetRequiredMasteryLevelForStarSlotUnlock(indexAmongLocked2))
			{
				availability = AvailabilityType.RequiredMasteryLevel;
			}
			else if (DewSave.profile.stardust < Dew.GetRequiredStardustForStarSlotUnlock(indexAmongLocked2))
			{
				availability = AvailabilityType.InsufficientStardust;
			}
			else
			{
				availability = AvailabilityType.Available;
			}
		}
		else
		{
			availability = AvailabilityType.Unlocked;
		}
		levelReqText.gameObject.SetActive(availability != AvailabilityType.Available);
		lockedObject.GetComponent<CanvasGroup>().alpha = ((availability == AvailabilityType.Available) ? 1f : 0.4f);
		if (index >= list.Count || string.IsNullOrEmpty(list[index].name))
		{
			emptyObject.SetActive(!isLocked);
			nonEmptyObject.SetActive(value: false);
			emptyIconImage.sprite = categoryIconSprites[(int)type];
			emptyIconImage.color = c.WithS(c.GetS() * 0.4f).WithA(0.175f);
			emptyIconImage.GetComponent<NicerOutline>().effectColor = c.WithA(0f);
			if (!isUnlocked)
			{
				emptyIconImage.color = emptyIconImage.color.WithA(0.1f);
			}
			Image[] array = emptyIconColoredImages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].color = emptyIconImage.color;
			}
			star = null;
		}
		else if (!(star != null) || !(star.GetType().Name == list[index].name))
		{
			_starIcon.transform.DOComplete();
			_starIcon.transform.DOPunchScale(Vector3.one * 0.25f, 0.35f);
			emptyObject.SetActive(value: false);
			nonEmptyObject.SetActive(value: true);
			star = DewResources.GetByShortTypeName<StarEffect>(list[index].name);
			_starIcon.Setup(star, -1f, isLocked: false);
			HeroSkill hs = h.GetComponent<HeroSkill>();
			incompatibleObject.SetActive(star.skillType != null && hs.loadoutQ[l.skillQ].GetType() != star.skillType && hs.loadoutR[l.skillR].GetType() != star.skillType);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		List<LoadoutStarItem> list = SingletonBehaviour<UI_Lobby_Constellations>.instance.loadout.GetStarList(type);
		if (!string.IsNullOrEmpty(list[index].name))
		{
			SingletonBehaviour<UI_Lobby_Constellations>.instance.StartDraggingSlot(type, index, DewResources.GetByShortTypeName<StarEffect>(list[index].name));
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		SingletonBehaviour<UI_Lobby_Constellations>.instance.EndDrag();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (SingletonBehaviour<UI_TooltipManager>.softInstance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.softInstance.UpdateTooltip();
		}
		SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.hoveredStarSlot = this;
		switch (availability)
		{
		case AvailabilityType.Unlocked:
			break;
		case AvailabilityType.Available:
			lockIconObject.SetActive(value: false);
			costDisplay.gameObject.SetActive(value: true);
			break;
		case AvailabilityType.AdjacentRequirement:
			lockIconObject.SetActive(value: true);
			costDisplay.gameObject.SetActive(value: false);
			break;
		case AvailabilityType.InsufficientStardust:
			lockIconObject.SetActive(value: false);
			costDisplay.gameObject.SetActive(value: true);
			break;
		case AvailabilityType.RequiredMasteryLevel:
			costDisplay.gameObject.SetActive(value: false);
			lockIconObject.SetActive(value: true);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (SingletonBehaviour<UI_TooltipManager>.softInstance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.softInstance.UpdateTooltip();
		}
		if (SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.hoveredStarSlot == this)
		{
			SingletonBehaviour<UI_Lobby_Constellations_HeroConstellation>.instance.hoveredStarSlot = null;
		}
		costDisplay.gameObject.SetActive(value: false);
		lockIconObject.gameObject.SetActive(value: true);
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		string category = DewLocalization.GetUIValue("Constellations_Category_" + type);
		switch (availability)
		{
		case AvailabilityType.Unlocked:
			if (star == null)
			{
				Show(string.Format(DewLocalization.GetUIValue("Constellation_Tooltip_EmptySlot"), category));
			}
			else
			{
				Show(UI_Lobby_Constellations_StarDetails.GetColoredPrefixedStarName(star));
			}
			break;
		case AvailabilityType.Available:
			Show(string.Format(DewLocalization.GetUIValue("Constellation_Tooltip_LockedSlot_Available"), category));
			break;
		case AvailabilityType.AdjacentRequirement:
			Show(string.Format(DewLocalization.GetUIValue("Constellation_Tooltip_LockedSlot_AdjacentRequirement"), category));
			break;
		case AvailabilityType.InsufficientStardust:
			Show(string.Format(DewLocalization.GetUIValue("Constellation_Tooltip_LockedSlot_InsufficientStardust"), category));
			break;
		case AvailabilityType.RequiredMasteryLevel:
		{
			HeroConstellationSettings s = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType).GetConstellationSettings(type);
			int indexAmongLocked = index - s.defaultCount;
			Show(string.Format(DewLocalization.GetUIValue("Constellation_Tooltip_LockedSlot_RequiredMasteryLevel"), category, DewLocalization.GetUIValue(DewPlayer.local.selectedHeroType + "_Name"), Dew.GetRequiredMasteryLevelForStarSlotUnlock(indexAmongLocked)));
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		void Show(string text)
		{
			tooltip.ShowRawTextTooltip(new TooltipSettings
			{
				getter = () => tooltipPivot.position,
				mode = TooltipPositionMode.Getter,
				pivot = new Vector2(0.5f, 0f)
			}, text);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (isLocked)
		{
			if (availability != AvailabilityType.Available)
			{
				return;
			}
			Hero hero = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType);
			string template = DewLocalization.GetUIValue("Constellations_PurchaseSlotConfirmation");
			string heroName = "<color=" + Dew.GetHex(hero.mainColor.WithV(1f).WithS(hero.mainColor.GetS() * 0.5f)) + ">" + DewLocalization.GetUIValue(DewPlayer.local.selectedHeroType + "_Name") + "</color>";
			Color catCol = Dew.GetStarCategoryColor(type);
			string categoryName = "<color=" + Dew.GetHex(catCol.WithS(catCol.GetS() * 0.5f).WithV(1f)) + ">" + DewLocalization.GetUIValue("Constellations_Category_" + type) + "</color>";
			HeroConstellationSettings s = DewResources.GetByShortTypeName<Hero>(DewPlayer.local.selectedHeroType).GetConstellationSettings(type);
			int indexAmongLocked = index - s.defaultCount;
			int price = Dew.GetRequiredStardustForStarSlotUnlock(indexAmongLocked);
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				owner = this,
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				rawContent = string.Format(template, heroName, categoryName, price),
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b != DewMessageSettings.ButtonType.Yes)
					{
						return;
					}
					DewEffect.PlayNew(fxUnlock);
					DewSave.profile.stardust -= price;
					DewSave.profile.spentStardust += price;
					DewSave.profile.heroUnlockedStarSlots[DewPlayer.local.selectedHeroType].Get(type).Add(index);
					DewSave.SaveProfile();
					Refresh();
					foreach (UI_Lobby_Constellations_HeroConstellation_StarSlot adjacentSlot in adjacentSlots)
					{
						adjacentSlot.Refresh();
					}
				}
			});
		}
		else if (star == null)
		{
			SingletonBehaviour<UI_Lobby_Constellations_StarList>.instance.SelectCategory(type);
		}
		else
		{
			SingletonBehaviour<UI_Lobby_Constellations_StarList>.instance.SelectStar(star);
		}
	}
}
