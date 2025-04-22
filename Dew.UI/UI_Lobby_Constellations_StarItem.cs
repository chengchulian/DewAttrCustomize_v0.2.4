using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Lobby_Constellations_StarItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public TextMeshProUGUI levelText;

	public CostDisplay costDisplay;

	public CanvasGroup skillDisplayCg;

	private UI_Lobby_Constellations_StarList _parentList;

	private UI_ToggleGroup _parentGroup;

	private UI_StarIcon _icon;

	private int _currentLevel;

	private bool _isLocked;

	public StarEffect star { get; private set; }

	public int index { get; private set; }

	private void Start()
	{
		SingletonBehaviour<UI_Lobby_Constellations>.instance.onIsDraggingChanged += new Action(OnIsDraggingChanged);
	}

	private void OnDestroy()
	{
		if (SingletonBehaviour<UI_Lobby_Constellations>.instance != null)
		{
			SingletonBehaviour<UI_Lobby_Constellations>.instance.onIsDraggingChanged -= new Action(OnIsDraggingChanged);
		}
	}

	private void OnIsDraggingChanged()
	{
		costDisplay.gameObject.SetActive(value: false);
		UI_Lobby_Constellations c = SingletonBehaviour<UI_Lobby_Constellations>.instance;
		_icon.GetComponent<CanvasGroup>().alpha = ((c.isDragging && !c.isDraggingFromSlot && c.draggingStar == star) ? 0.1f : 1f);
	}

	public void Setup(StarEffect s, int listIndex)
	{
		_icon = GetComponentInChildren<UI_StarIcon>();
		GetComponent<UI_Toggle>().doNotToggleOnClick = true;
		index = listIndex;
		_parentList = GetComponentInParent<UI_Lobby_Constellations_StarList>();
		_parentGroup = GetComponentInParent<UI_ToggleGroup>();
		star = s;
		Type heroType = s.heroType;
		int currentReqLevel = ((heroType != null) ? DewSave.profile.heroMasteries[heroType.Name].currentLevel : DewSave.profile.totalMasteryLevel);
		_isLocked = currentReqLevel < s.requiredLevel && _currentLevel <= 0;
		skillDisplayCg.alpha = (_isLocked ? 0.5f : 1f);
		if (_isLocked)
		{
			levelText.color = new Color(1f, 1f, 1f, 0.2f);
		}
		else
		{
			levelText.color = new Color(1f, 1f, 1f, 0.7f);
		}
		GetComponent<UI_Toggle>().index = listIndex;
		Refresh();
	}

	public void Refresh()
	{
		_currentLevel = DewSave.profile.newStars[star.GetType().Name].level;
		levelText.text = $"{_currentLevel}/{star.maxStarLevel}";
		if (_currentLevel < star.maxStarLevel)
		{
			costDisplay.Setup(new Cost
			{
				stardust = star.price[_currentLevel]
			});
		}
		costDisplay.gameObject.SetActive(value: false);
		_icon.Setup(star, -1f, _isLocked);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_parentList.hoveredIndex = index;
		costDisplay.gameObject.SetActive(_currentLevel < star.maxStarLevel && !SingletonBehaviour<UI_Lobby_Constellations>.instance.isDragging);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_parentList.hoveredIndex == index)
		{
			_parentList.hoveredIndex = -1;
		}
		costDisplay.gameObject.SetActive(value: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_parentGroup.currentIndex = index;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		OnPointerClick(eventData);
		if (DewSave.profile.newStars[star.GetType().Name].level > 0)
		{
			SingletonBehaviour<UI_Lobby_Constellations>.instance.StartDragStar(star);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		SingletonBehaviour<UI_Lobby_Constellations>.instance.EndDrag();
	}

	public void OnDrag(PointerEventData eventData)
	{
	}
}
