using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("UI/Select Field", 58)]
[RequireComponent(typeof(Image))]
public class UISelectField : Toggle
{
	public enum Direction
	{
		Auto,
		Down,
		Up
	}

	public enum VisualState
	{
		Normal,
		Highlighted,
		Pressed,
		Active,
		ActiveHighlighted,
		ActivePressed,
		Disabled
	}

	public enum ListAnimationType
	{
		None,
		Fade,
		Animation
	}

	public enum OptionTextTransitionType
	{
		None,
		CrossFade
	}

	public enum OptionTextEffectType
	{
		None,
		Shadow,
		Outline
	}

	[Serializable]
	public class ChangeEvent : UnityEvent<int, string>
	{
	}

	[Serializable]
	public class TransitionEvent : UnityEvent<VisualState, bool>
	{
	}

	[HideInInspector]
	[SerializeField]
	private string m_SelectedItem;

	private List<UISelectField_Option> m_OptionObjects = new List<UISelectField_Option>();

	private VisualState m_CurrentVisualState;

	private bool m_PointerWasUsedOnOption;

	private GameObject m_ListObject;

	private ScrollRect m_ScrollRect;

	private GameObject m_ListContentObject;

	private CanvasGroup m_ListCanvasGroup;

	private Vector2 m_LastListSize = Vector2.zero;

	private GameObject m_StartSeparatorObject;

	private Navigation.Mode m_LastNavigationMode;

	private GameObject m_LastSelectedGameObject;

	private GameObject m_Blocker;

	[SerializeField]
	private Direction m_Direction;

	[SerializeField]
	[FormerlySerializedAs("options")]
	private List<string> m_Options = new List<string>();

	[SerializeField]
	private Text m_LabelText;

	public new ColorBlockExtended colors = ColorBlockExtended.defaultColorBlock;

	public new SpriteStateExtended spriteState;

	public new AnimationTriggersExtended animationTriggers = new AnimationTriggersExtended();

	public Sprite listBackgroundSprite;

	public Image.Type listBackgroundSpriteType = Image.Type.Sliced;

	public Color listBackgroundColor = Color.white;

	public RectOffset listMargins;

	public RectOffset listPadding;

	public float listSpacing;

	public ListAnimationType listAnimationType = ListAnimationType.Fade;

	public float listAnimationDuration = 0.1f;

	public RuntimeAnimatorController listAnimatorController;

	public string listAnimationOpenTrigger = "Open";

	public string listAnimationCloseTrigger = "Close";

	public bool allowScrollRect = true;

	public ScrollRect.MovementType scrollMovementType = ScrollRect.MovementType.Clamped;

	public float scrollElasticity = 0.1f;

	public bool scrollInertia;

	public float scrollDecelerationRate = 0.135f;

	public float scrollSensitivity = 1f;

	public int scrollMinOptions = 5;

	public float scrollListHeight = 512f;

	public GameObject scrollBarPrefab;

	public float scrollbarSpacing = 34f;

	public Font optionFont = FontData.defaultFontData.font;

	public int optionFontSize = FontData.defaultFontData.fontSize;

	public FontStyle optionFontStyle = FontData.defaultFontData.fontStyle;

	public Color optionColor = Color.white;

	public OptionTextTransitionType optionTextTransitionType = OptionTextTransitionType.CrossFade;

	public ColorBlockExtended optionTextTransitionColors = ColorBlockExtended.defaultColorBlock;

	public RectOffset optionPadding;

	public OptionTextEffectType optionTextEffectType;

	public Color optionTextEffectColor = new Color(0f, 0f, 0f, 128f);

	public Vector2 optionTextEffectDistance = new Vector2(1f, -1f);

	public bool optionTextEffectUseGraphicAlpha = true;

	public Sprite optionBackgroundSprite;

	public Color optionBackgroundSpriteColor = Color.white;

	public Image.Type optionBackgroundSpriteType = Image.Type.Sliced;

	public Transition optionBackgroundTransitionType;

	public ColorBlockExtended optionBackgroundTransColors = ColorBlockExtended.defaultColorBlock;

	public SpriteStateExtended optionBackgroundSpriteStates;

	public AnimationTriggersExtended optionBackgroundAnimationTriggers = new AnimationTriggersExtended();

	public RuntimeAnimatorController optionBackgroundAnimatorController;

	public Sprite optionHoverOverlay;

	public Color optionHoverOverlayColor = Color.white;

	public ColorBlock optionHoverOverlayColorBlock = ColorBlock.defaultColorBlock;

	public Sprite optionPressOverlay;

	public Color optionPressOverlayColor = Color.white;

	public ColorBlock optionPressOverlayColorBlock = ColorBlock.defaultColorBlock;

	public Sprite listSeparatorSprite;

	public Image.Type listSeparatorType;

	public Color listSeparatorColor = Color.white;

	public float listSeparatorHeight;

	public bool startSeparator;

	public ChangeEvent onChange = new ChangeEvent();

	public TransitionEvent onTransition = new TransitionEvent();

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

	public Direction direction
	{
		get
		{
			return m_Direction;
		}
		set
		{
			m_Direction = value;
		}
	}

	public List<string> options => m_Options;

	public string value
	{
		get
		{
			return m_SelectedItem;
		}
		set
		{
			SelectOption(value);
		}
	}

	public int selectedOptionIndex => GetOptionIndex(m_SelectedItem);

	public bool IsOpen => base.isOn;

	protected UISelectField()
	{
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_FloatTweenRunner.Init(this);
	}

	protected override void Awake()
	{
		base.Awake();
		if (base.targetGraphic == null)
		{
			base.targetGraphic = GetComponent<Image>();
		}
	}

	protected override void Start()
	{
		base.Start();
		toggleTransition = ToggleTransition.None;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		onValueChanged.AddListener(OnToggleValueChanged);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		onValueChanged.RemoveListener(OnToggleValueChanged);
		base.isOn = false;
		DoStateTransition(SelectionState.Disabled, instant: true);
	}

	public void Open()
	{
		base.isOn = true;
	}

	public void Close()
	{
		base.isOn = false;
	}

	public int GetOptionIndex(string optionValue)
	{
		if (m_Options != null && m_Options.Count > 0 && !string.IsNullOrEmpty(optionValue))
		{
			for (int i = 0; i < m_Options.Count; i++)
			{
				if (optionValue.Equals(m_Options[i], StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void SelectOptionByIndex(int index)
	{
		if (IsOpen)
		{
			UISelectField_Option option = m_OptionObjects[index];
			if (option != null)
			{
				option.isOn = true;
			}
		}
		else
		{
			m_SelectedItem = m_Options[index];
			TriggerChangeEvent();
		}
	}

	public void SelectOption(string optionValue)
	{
		if (!string.IsNullOrEmpty(optionValue))
		{
			int index = GetOptionIndex(optionValue);
			if (index >= 0 && index < m_Options.Count)
			{
				SelectOptionByIndex(index);
			}
		}
	}

	public void AddOption(string optionValue)
	{
		if (m_Options != null)
		{
			m_Options.Add(optionValue);
			OptionListChanged();
		}
	}

	public void AddOptionAtIndex(string optionValue, int index)
	{
		if (m_Options != null)
		{
			if (index >= m_Options.Count)
			{
				m_Options.Add(optionValue);
			}
			else
			{
				m_Options.Insert(index, optionValue);
			}
			OptionListChanged();
		}
	}

	public void RemoveOption(string optionValue)
	{
		if (m_Options != null && m_Options.Contains(optionValue))
		{
			m_Options.Remove(optionValue);
			OptionListChanged();
			ValidateSelectedOption();
		}
	}

	public void RemoveOptionAtIndex(int index)
	{
		if (m_Options != null && index >= 0 && index < m_Options.Count)
		{
			m_Options.RemoveAt(index);
			OptionListChanged();
			ValidateSelectedOption();
		}
	}

	public void ClearOptions()
	{
		if (m_Options != null)
		{
			m_Options.Clear();
			OptionListChanged();
		}
	}

	public void ValidateSelectedOption()
	{
		if (m_Options != null && !m_Options.Contains(m_SelectedItem))
		{
			SelectOptionByIndex(0);
		}
	}

	public void OnOptionSelect(string option)
	{
		if (!string.IsNullOrEmpty(option))
		{
			string selectedItem = m_SelectedItem;
			m_SelectedItem = option;
			if (!selectedItem.Equals(m_SelectedItem))
			{
				TriggerChangeEvent();
			}
			if (IsOpen && m_PointerWasUsedOnOption)
			{
				m_PointerWasUsedOnOption = false;
				Close();
				base.OnDeselect(new BaseEventData(EventSystem.current));
			}
		}
	}

	public void OnOptionPointerUp(BaseEventData eventData)
	{
		m_PointerWasUsedOnOption = true;
	}

	protected virtual void TriggerChangeEvent()
	{
		if (m_LabelText != null)
		{
			m_LabelText.text = m_SelectedItem;
		}
		if (onChange != null)
		{
			onChange.Invoke(selectedOptionIndex, m_SelectedItem);
		}
	}

	private void OnToggleValueChanged(bool state)
	{
		if (Application.isPlaying)
		{
			DoStateTransition(base.currentSelectionState, instant: false);
			ToggleList(base.isOn);
			if (!base.isOn && m_Blocker != null)
			{
				global::UnityEngine.Object.Destroy(m_Blocker);
			}
		}
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		if (m_ListObject != null && m_ListObject.GetComponent<UISelectField_List>().IsHighlighted(eventData))
		{
			return;
		}
		foreach (UISelectField_Option optionObject in m_OptionObjects)
		{
			if (optionObject.IsHighlighted(eventData))
			{
				return;
			}
		}
		Close();
		base.OnDeselect(eventData);
	}

	public override void OnMove(AxisEventData eventData)
	{
		if (IsOpen)
		{
			int prevIndex = selectedOptionIndex - 1;
			int nextIndex = selectedOptionIndex + 1;
			switch (eventData.moveDir)
			{
			case MoveDirection.Up:
				if (prevIndex >= 0)
				{
					SelectOptionByIndex(prevIndex);
				}
				break;
			case MoveDirection.Down:
				if (nextIndex < m_Options.Count)
				{
					SelectOptionByIndex(nextIndex);
				}
				break;
			}
			eventData.Use();
		}
		else
		{
			base.OnMove(eventData);
		}
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		if (base.gameObject.activeInHierarchy)
		{
			Color color = colors.normalColor;
			Sprite newSprite = null;
			string triggername = animationTriggers.normalTrigger;
			switch (state)
			{
			case SelectionState.Disabled:
				m_CurrentVisualState = VisualState.Disabled;
				color = colors.disabledColor;
				newSprite = spriteState.disabledSprite;
				triggername = animationTriggers.disabledTrigger;
				break;
			case SelectionState.Normal:
				m_CurrentVisualState = (base.isOn ? VisualState.Active : VisualState.Normal);
				color = (base.isOn ? colors.activeColor : colors.normalColor);
				newSprite = (base.isOn ? spriteState.activeSprite : null);
				triggername = (base.isOn ? animationTriggers.activeTrigger : animationTriggers.normalTrigger);
				break;
			case SelectionState.Highlighted:
			case SelectionState.Selected:
				m_CurrentVisualState = ((!base.isOn) ? VisualState.Highlighted : VisualState.ActiveHighlighted);
				color = (base.isOn ? colors.activeHighlightedColor : colors.highlightedColor);
				newSprite = (base.isOn ? spriteState.activeHighlightedSprite : spriteState.highlightedSprite);
				triggername = (base.isOn ? animationTriggers.activeHighlightedTrigger : animationTriggers.highlightedTrigger);
				break;
			case SelectionState.Pressed:
				m_CurrentVisualState = (base.isOn ? VisualState.ActivePressed : VisualState.Pressed);
				color = (base.isOn ? colors.activePressedColor : colors.pressedColor);
				newSprite = (base.isOn ? spriteState.activePressedSprite : spriteState.pressedSprite);
				triggername = (base.isOn ? animationTriggers.activePressedTrigger : animationTriggers.pressedTrigger);
				break;
			}
			switch (base.transition)
			{
			case Transition.ColorTint:
				StartColorTween(color * colors.colorMultiplier, instant ? 0f : colors.fadeDuration);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(newSprite);
				break;
			case Transition.Animation:
				TriggerAnimation(triggername);
				break;
			}
			if (onTransition != null)
			{
				onTransition.Invoke(m_CurrentVisualState, instant);
			}
		}
	}

	private void StartColorTween(Color color, float duration)
	{
		if (!(base.targetGraphic == null))
		{
			base.targetGraphic.CrossFadeColor(color, duration, ignoreTimeScale: true, useAlpha: true);
		}
	}

	private void DoSpriteSwap(Sprite newSprite)
	{
		Image image = base.targetGraphic as Image;
		if (!(image == null))
		{
			image.overrideSprite = newSprite;
		}
	}

	private void TriggerAnimation(string trigger)
	{
		if (!(base.animator == null) && base.animator.enabled && base.animator.isActiveAndEnabled && !(base.animator.runtimeAnimatorController == null) && base.animator.hasBoundPlayables && !string.IsNullOrEmpty(trigger))
		{
			base.animator.ResetTrigger(animationTriggers.normalTrigger);
			base.animator.ResetTrigger(animationTriggers.pressedTrigger);
			base.animator.ResetTrigger(animationTriggers.highlightedTrigger);
			base.animator.ResetTrigger(animationTriggers.activeTrigger);
			base.animator.ResetTrigger(animationTriggers.activeHighlightedTrigger);
			base.animator.ResetTrigger(animationTriggers.activePressedTrigger);
			base.animator.ResetTrigger(animationTriggers.disabledTrigger);
			base.animator.SetTrigger(trigger);
		}
	}

	protected virtual void ToggleList(bool state)
	{
		if (!IsActive())
		{
			return;
		}
		if (m_ListObject == null)
		{
			CreateList();
		}
		if (m_ListObject == null)
		{
			return;
		}
		if (m_ListCanvasGroup != null)
		{
			m_ListCanvasGroup.blocksRaycasts = state;
		}
		if (state)
		{
			m_LastNavigationMode = base.navigation.mode;
			m_LastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			Navigation newNav = base.navigation;
			newNav.mode = Navigation.Mode.Vertical;
			base.navigation = newNav;
			EventSystem.current.SetSelectedGameObject(base.gameObject);
		}
		else
		{
			Navigation newNav2 = base.navigation;
			newNav2.mode = m_LastNavigationMode;
			base.navigation = newNav2;
			if (!EventSystem.current.alreadySelecting && m_LastSelectedGameObject != null)
			{
				EventSystem.current.SetSelectedGameObject(m_LastSelectedGameObject);
			}
		}
		if (state)
		{
			UIUtility.BringToFront(m_ListObject);
		}
		if (listAnimationType == ListAnimationType.None || listAnimationType == ListAnimationType.Fade)
		{
			float targetAlpha = (state ? 1f : 0f);
			TweenListAlpha(targetAlpha, (listAnimationType == ListAnimationType.Fade) ? listAnimationDuration : 0f, ignoreTimeScale: true);
		}
		else if (listAnimationType == ListAnimationType.Animation)
		{
			TriggerListAnimation(state ? listAnimationOpenTrigger : listAnimationCloseTrigger);
		}
	}

	protected void CreateList()
	{
		m_LastListSize = Vector2.zero;
		m_OptionObjects.Clear();
		m_ListObject = new GameObject("UISelectField - List", typeof(RectTransform));
		m_ListObject.layer = base.gameObject.layer;
		m_ListObject.transform.SetParent(base.transform, worldPositionStays: false);
		UISelectField_List listComp = m_ListObject.AddComponent<UISelectField_List>();
		m_ListObject.AddComponent<UIAlwaysOnTop>().order = 99998;
		m_ListCanvasGroup = m_ListObject.AddComponent<CanvasGroup>();
		RectTransform rect = m_ListObject.transform as RectTransform;
		rect.localScale = new Vector3(1f, 1f, 1f);
		rect.localPosition = Vector3.zero;
		rect.anchorMin = Vector2.zero;
		rect.anchorMax = Vector2.zero;
		rect.pivot = new Vector2(0f, 1f);
		rect.anchoredPosition = new Vector3(listMargins.left, (float)listMargins.top * -1f, 0f);
		float width = (base.transform as RectTransform).sizeDelta.x;
		width = ((listMargins.left <= 0) ? (width + (float)Math.Abs(listMargins.left)) : (width - (float)listMargins.left));
		width = ((listMargins.right <= 0) ? (width + (float)Math.Abs(listMargins.right)) : (width - (float)listMargins.right));
		rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		listComp.onDimensionsChange.AddListener(ListDimensionsChanged);
		Image image = m_ListObject.AddComponent<Image>();
		if (listBackgroundSprite != null)
		{
			image.sprite = listBackgroundSprite;
		}
		image.type = listBackgroundSpriteType;
		image.color = listBackgroundColor;
		if (allowScrollRect && m_Options.Count >= scrollMinOptions)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollListHeight);
			GameObject scrollRectGo = new GameObject("Scroll Rect", typeof(RectTransform));
			scrollRectGo.layer = m_ListObject.layer;
			scrollRectGo.transform.SetParent(m_ListObject.transform, worldPositionStays: false);
			RectTransform obj = scrollRectGo.transform as RectTransform;
			obj.localScale = new Vector3(1f, 1f, 1f);
			obj.localPosition = Vector3.zero;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.pivot = new Vector2(0f, 1f);
			obj.anchoredPosition = Vector2.zero;
			obj.offsetMin = new Vector2(listPadding.left, listPadding.bottom);
			obj.offsetMax = new Vector2((float)listPadding.right * -1f, (float)listPadding.top * -1f);
			m_ScrollRect = scrollRectGo.AddComponent<ScrollRect>();
			m_ScrollRect.horizontal = false;
			m_ScrollRect.movementType = scrollMovementType;
			m_ScrollRect.elasticity = scrollElasticity;
			m_ScrollRect.inertia = scrollInertia;
			m_ScrollRect.decelerationRate = scrollDecelerationRate;
			m_ScrollRect.scrollSensitivity = scrollSensitivity;
			GameObject obj2 = new GameObject("View Port", typeof(RectTransform));
			obj2.layer = m_ListObject.layer;
			obj2.transform.SetParent(scrollRectGo.transform, worldPositionStays: false);
			RectTransform viewPortRect = obj2.transform as RectTransform;
			viewPortRect.localScale = new Vector3(1f, 1f, 1f);
			viewPortRect.localPosition = Vector3.zero;
			viewPortRect.anchorMin = Vector2.zero;
			viewPortRect.anchorMax = Vector2.one;
			viewPortRect.pivot = new Vector2(0f, 1f);
			viewPortRect.anchoredPosition = Vector2.zero;
			viewPortRect.offsetMin = Vector2.zero;
			viewPortRect.offsetMax = Vector2.zero;
			obj2.AddComponent<Image>().raycastTarget = false;
			obj2.AddComponent<Mask>().showMaskGraphic = false;
			m_ListContentObject = new GameObject("Content", typeof(RectTransform));
			m_ListContentObject.layer = m_ListObject.layer;
			m_ListContentObject.transform.SetParent(viewPortRect, worldPositionStays: false);
			RectTransform contentRect = m_ListContentObject.transform as RectTransform;
			contentRect.localScale = new Vector3(1f, 1f, 1f);
			contentRect.localPosition = Vector3.zero;
			contentRect.anchorMin = new Vector2(0f, 1f);
			contentRect.anchorMax = new Vector2(0f, 1f);
			contentRect.pivot = new Vector2(0f, 1f);
			contentRect.anchoredPosition = Vector2.zero;
			contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.sizeDelta.x);
			m_ListContentObject.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
			m_ListContentObject.AddComponent<UISelectField_List>().onDimensionsChange.AddListener(ScrollContentDimensionsChanged);
			m_ScrollRect.content = contentRect;
			m_ScrollRect.viewport = viewPortRect;
			if (scrollBarPrefab != null)
			{
				GameObject scrollBarGo = global::UnityEngine.Object.Instantiate(scrollBarPrefab, scrollRectGo.transform);
				m_ScrollRect.verticalScrollbar = scrollBarGo.GetComponent<Scrollbar>();
				m_ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
				m_ScrollRect.verticalScrollbarSpacing = scrollbarSpacing;
			}
			m_ListContentObject.AddComponent<VerticalLayoutGroup>();
		}
		else
		{
			m_ListContentObject = m_ListObject;
			VerticalLayoutGroup verticalLayoutGroup = m_ListContentObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = listPadding;
			verticalLayoutGroup.spacing = listSpacing;
		}
		m_ListContentObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		ToggleGroup toggleGroup = m_ListObject.AddComponent<ToggleGroup>();
		for (int i = 0; i < m_Options.Count; i++)
		{
			if (i == 0 && startSeparator)
			{
				m_StartSeparatorObject = CreateSeparator(i - 1);
			}
			CreateOption(i, toggleGroup);
			if (i < m_Options.Count - 1)
			{
				CreateSeparator(i);
			}
		}
		if (listAnimationType == ListAnimationType.None || listAnimationType == ListAnimationType.Fade)
		{
			m_ListCanvasGroup.alpha = 0f;
		}
		else if (listAnimationType == ListAnimationType.Animation)
		{
			m_ListObject.AddComponent<Animator>().runtimeAnimatorController = listAnimatorController;
			listComp.SetTriggers(listAnimationOpenTrigger, listAnimationCloseTrigger);
			listComp.onAnimationFinish.AddListener(OnListAnimationFinish);
		}
		if (base.navigation.mode == Navigation.Mode.None)
		{
			CreateBlocker();
		}
		if (allowScrollRect && m_Options.Count >= scrollMinOptions)
		{
			ListDimensionsChanged();
		}
	}

	protected virtual void CreateBlocker()
	{
		GameObject blocker = new GameObject("Blocker");
		RectTransform rectTransform = blocker.AddComponent<RectTransform>();
		rectTransform.SetParent(base.transform, worldPositionStays: false);
		rectTransform.localScale = Vector3.one;
		rectTransform.localPosition = Vector3.zero;
		blocker.AddComponent<Image>().color = Color.clear;
		blocker.AddComponent<Button>().onClick.AddListener(Close);
		blocker.AddComponent<UIAlwaysOnTop>().order = 99997;
		UIUtility.BringToFront(blocker);
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		m_Blocker = blocker;
	}

	protected void CreateOption(int index, ToggleGroup toggleGroup)
	{
		if (m_ListContentObject == null)
		{
			return;
		}
		GameObject optionObject = new GameObject("Option " + index, typeof(RectTransform));
		optionObject.layer = base.gameObject.layer;
		optionObject.transform.SetParent(m_ListContentObject.transform, worldPositionStays: false);
		optionObject.transform.localScale = new Vector3(1f, 1f, 1f);
		optionObject.transform.localPosition = Vector3.zero;
		UISelectField_Option optionComp = optionObject.AddComponent<UISelectField_Option>();
		if (optionBackgroundSprite != null)
		{
			Image image = optionObject.AddComponent<Image>();
			image.sprite = optionBackgroundSprite;
			image.type = optionBackgroundSpriteType;
			image.color = optionBackgroundSpriteColor;
			optionComp.targetGraphic = image;
		}
		if (optionBackgroundTransitionType == Transition.Animation)
		{
			optionObject.AddComponent<Animator>().runtimeAnimatorController = optionBackgroundAnimatorController;
		}
		optionObject.AddComponent<VerticalLayoutGroup>().padding = optionPadding;
		GameObject textObject = new GameObject("Label", typeof(RectTransform));
		textObject.transform.SetParent(optionObject.transform, worldPositionStays: false);
		textObject.transform.localScale = Vector3.one;
		textObject.transform.localPosition = Vector3.zero;
		(textObject.transform as RectTransform).pivot = new Vector2(0f, 1f);
		Text text = textObject.AddComponent<Text>();
		text.font = optionFont;
		text.fontSize = optionFontSize;
		text.fontStyle = optionFontStyle;
		text.color = optionColor;
		if (m_Options != null)
		{
			text.text = m_Options[index];
		}
		if (optionTextTransitionType == OptionTextTransitionType.CrossFade)
		{
			text.canvasRenderer.SetColor(optionTextTransitionColors.normalColor);
		}
		if (optionTextEffectType != 0)
		{
			if (optionTextEffectType == OptionTextEffectType.Shadow)
			{
				Shadow shadow = textObject.AddComponent<Shadow>();
				shadow.effectColor = optionTextEffectColor;
				shadow.effectDistance = optionTextEffectDistance;
				shadow.useGraphicAlpha = optionTextEffectUseGraphicAlpha;
			}
			else if (optionTextEffectType == OptionTextEffectType.Outline)
			{
				Outline outline = textObject.AddComponent<Outline>();
				outline.effectColor = optionTextEffectColor;
				outline.effectDistance = optionTextEffectDistance;
				outline.useGraphicAlpha = optionTextEffectUseGraphicAlpha;
			}
		}
		if (optionHoverOverlay != null)
		{
			GameObject obj = new GameObject("Hover Overlay", typeof(RectTransform));
			obj.layer = base.gameObject.layer;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.AddComponent<LayoutElement>().ignoreLayout = true;
			obj.transform.SetParent(optionObject.transform, worldPositionStays: false);
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			Image hoImage = obj.AddComponent<Image>();
			hoImage.sprite = optionHoverOverlay;
			hoImage.color = optionHoverOverlayColor;
			hoImage.type = Image.Type.Sliced;
			(obj.transform as RectTransform).pivot = new Vector2(0f, 1f);
			(obj.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			(obj.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
			(obj.transform as RectTransform).offsetMin = new Vector2(0f, 0f);
			(obj.transform as RectTransform).offsetMax = new Vector2(0f, 0f);
			UISelectField_OptionOverlay uISelectField_OptionOverlay = optionObject.AddComponent<UISelectField_OptionOverlay>();
			uISelectField_OptionOverlay.targetGraphic = hoImage;
			uISelectField_OptionOverlay.transition = UISelectField_OptionOverlay.Transition.ColorTint;
			uISelectField_OptionOverlay.colorBlock = optionHoverOverlayColorBlock;
			uISelectField_OptionOverlay.InternalEvaluateAndTransitionToNormalState(instant: true);
		}
		if (optionPressOverlay != null)
		{
			GameObject obj2 = new GameObject("Press Overlay", typeof(RectTransform));
			obj2.layer = base.gameObject.layer;
			obj2.transform.localScale = Vector3.one;
			obj2.transform.localPosition = Vector3.zero;
			obj2.AddComponent<LayoutElement>().ignoreLayout = true;
			obj2.transform.SetParent(optionObject.transform, worldPositionStays: false);
			obj2.transform.localScale = new Vector3(1f, 1f, 1f);
			Image poImage = obj2.AddComponent<Image>();
			poImage.sprite = optionPressOverlay;
			poImage.color = optionPressOverlayColor;
			poImage.type = Image.Type.Sliced;
			(obj2.transform as RectTransform).pivot = new Vector2(0f, 1f);
			(obj2.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			(obj2.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
			(obj2.transform as RectTransform).offsetMin = new Vector2(0f, 0f);
			(obj2.transform as RectTransform).offsetMax = new Vector2(0f, 0f);
			UISelectField_OptionOverlay uISelectField_OptionOverlay2 = optionObject.AddComponent<UISelectField_OptionOverlay>();
			uISelectField_OptionOverlay2.targetGraphic = poImage;
			uISelectField_OptionOverlay2.transition = UISelectField_OptionOverlay.Transition.ColorTint;
			uISelectField_OptionOverlay2.colorBlock = optionPressOverlayColorBlock;
			uISelectField_OptionOverlay2.InternalEvaluateAndTransitionToNormalState(instant: true);
		}
		optionComp.Initialize(this, text);
		if (index == selectedOptionIndex)
		{
			optionComp.isOn = true;
		}
		if (toggleGroup != null)
		{
			optionComp.group = toggleGroup;
		}
		optionComp.onSelectOption.AddListener(OnOptionSelect);
		optionComp.onPointerUp.AddListener(OnOptionPointerUp);
		if (m_OptionObjects != null)
		{
			m_OptionObjects.Add(optionComp);
		}
	}

	protected GameObject CreateSeparator(int index)
	{
		if (m_ListContentObject == null || listSeparatorSprite == null)
		{
			return null;
		}
		GameObject obj = new GameObject("Separator " + index, typeof(RectTransform));
		obj.transform.SetParent(m_ListContentObject.transform, worldPositionStays: false);
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		Image obj2 = obj.AddComponent<Image>();
		obj2.sprite = listSeparatorSprite;
		obj2.type = listSeparatorType;
		obj2.color = listSeparatorColor;
		obj.AddComponent<LayoutElement>().preferredHeight = ((listSeparatorHeight > 0f) ? listSeparatorHeight : listSeparatorSprite.rect.height);
		return obj;
	}

	protected virtual void ListCleanup()
	{
		if (m_ListObject != null)
		{
			global::UnityEngine.Object.Destroy(m_ListObject);
		}
		m_OptionObjects.Clear();
	}

	public virtual void PositionListForDirection(Direction direction)
	{
		if (m_ListObject == null)
		{
			return;
		}
		RectTransform listRect = m_ListObject.transform as RectTransform;
		if (direction == Direction.Auto)
		{
			Vector3[] listWorldCorner = new Vector3[4];
			listRect.GetWorldCorners(listWorldCorner);
			direction = ((!(RectTransformUtility.WorldToScreenPoint(Camera.main, listWorldCorner[0]).y < 0f)) ? Direction.Down : Direction.Up);
		}
		if (direction == Direction.Down)
		{
			listRect.SetParent(base.transform, worldPositionStays: true);
			listRect.pivot = new Vector2(0f, 1f);
			listRect.anchorMin = new Vector2(0f, 0f);
			listRect.anchorMax = new Vector2(0f, 0f);
			listRect.anchoredPosition = new Vector2(listRect.anchoredPosition.x, (float)listMargins.top * -1f);
			UIUtility.BringToFront(listRect.gameObject);
			return;
		}
		listRect.SetParent(base.transform, worldPositionStays: true);
		listRect.pivot = new Vector2(0f, 0f);
		listRect.anchorMin = new Vector2(0f, 1f);
		listRect.anchorMax = new Vector2(0f, 1f);
		listRect.anchoredPosition = new Vector2(listRect.anchoredPosition.x, listMargins.bottom);
		if (m_StartSeparatorObject != null)
		{
			m_StartSeparatorObject.transform.SetAsLastSibling();
		}
		UIUtility.BringToFront(listRect.gameObject);
	}

	protected virtual void ListDimensionsChanged()
	{
		if (IsActive() && !(m_ListObject == null) && !m_LastListSize.Equals((m_ListObject.transform as RectTransform).sizeDelta))
		{
			m_LastListSize = (m_ListObject.transform as RectTransform).sizeDelta;
			PositionListForDirection(m_Direction);
		}
	}

	protected virtual void ScrollContentDimensionsChanged()
	{
		if (IsActive() && !(m_ScrollRect == null))
		{
			float optionPosition = m_ScrollRect.content.sizeDelta.y / (float)m_Options.Count * (float)selectedOptionIndex;
			m_ScrollRect.content.anchoredPosition = new Vector2(m_ScrollRect.content.anchoredPosition.x, optionPosition);
		}
	}

	protected virtual void OptionListChanged()
	{
	}

	private void TweenListAlpha(float targetAlpha, float duration, bool ignoreTimeScale)
	{
		if (!(m_ListCanvasGroup == null))
		{
			float currentAlpha = m_ListCanvasGroup.alpha;
			if (!currentAlpha.Equals(targetAlpha))
			{
				FloatTween floatTween = default(FloatTween);
				floatTween.duration = duration;
				floatTween.startFloat = currentAlpha;
				floatTween.targetFloat = targetAlpha;
				FloatTween floatTween2 = floatTween;
				floatTween2.AddOnChangedCallback(SetListAlpha);
				floatTween2.AddOnFinishCallback(OnListTweenFinished);
				floatTween2.ignoreTimeScale = ignoreTimeScale;
				m_FloatTweenRunner.StartTween(floatTween2);
			}
		}
	}

	private void SetListAlpha(float alpha)
	{
		if (!(m_ListCanvasGroup == null))
		{
			m_ListCanvasGroup.alpha = alpha;
		}
	}

	private void TriggerListAnimation(string trigger)
	{
		if (!(m_ListObject == null) && !string.IsNullOrEmpty(trigger))
		{
			Animator animator = m_ListObject.GetComponent<Animator>();
			if (!(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables)
			{
				animator.ResetTrigger(listAnimationOpenTrigger);
				animator.ResetTrigger(listAnimationCloseTrigger);
				animator.SetTrigger(trigger);
			}
		}
	}

	protected virtual void OnListTweenFinished()
	{
		if (!IsOpen)
		{
			ListCleanup();
		}
	}

	protected virtual void OnListAnimationFinish(UISelectField_List.State state)
	{
		if (state == UISelectField_List.State.Closed && !IsOpen)
		{
			ListCleanup();
		}
	}
}
