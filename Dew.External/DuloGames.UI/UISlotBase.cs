using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Icon Slots/Base Slot")]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class UISlotBase : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	public enum Transition
	{
		None,
		ColorTint,
		SpriteSwap,
		Animation
	}

	public enum DragKeyModifier
	{
		None,
		Control,
		Alt,
		Shift
	}

	protected GameObject m_CurrentDraggedObject;

	protected RectTransform m_CurrentDraggingPlane;

	public Graphic iconGraphic;

	[SerializeField]
	[Tooltip("The game object that should be cloned on drag.")]
	private GameObject m_CloneTarget;

	[SerializeField]
	[Tooltip("Should the drag and drop functionallty be enabled.")]
	private bool m_DragAndDropEnabled = true;

	[SerializeField]
	[Tooltip("If set to static the slot won't be unassigned when drag and drop is preformed.")]
	private bool m_IsStatic;

	[SerializeField]
	[Tooltip("Should the icon assigned to the slot be throwable.")]
	private bool m_AllowThrowAway = true;

	[SerializeField]
	[Tooltip("The key which should be held down in order to begin the drag.")]
	private DragKeyModifier m_DragKeyModifier;

	[SerializeField]
	[Tooltip("Should the tooltip functionallty be enabled.")]
	private bool m_TooltipEnabled = true;

	[SerializeField]
	[Tooltip("How long of a delay to expect before showing the tooltip.")]
	private float m_TooltipDelay = 1f;

	public Transition hoverTransition;

	public Graphic hoverTargetGraphic;

	public Color hoverNormalColor = Color.white;

	public Color hoverHighlightColor = Color.white;

	public float hoverTransitionDuration = 0.15f;

	public Sprite hoverOverrideSprite;

	public string hoverNormalTrigger = "Normal";

	public string hoverHighlightTrigger = "Highlighted";

	public Transition pressTransition;

	public Graphic pressTargetGraphic;

	public Color pressNormalColor = Color.white;

	public Color pressPressColor = new Color(0.6117f, 0.6117f, 0.6117f, 1f);

	public float pressTransitionDuration = 0.15f;

	public Sprite pressOverrideSprite;

	public string pressNormalTrigger = "Normal";

	public string pressPressTrigger = "Pressed";

	[SerializeField]
	[Tooltip("Should the pressed state transition to normal state instantly.")]
	private bool m_PressTransitionInstaOut = true;

	[SerializeField]
	[Tooltip("Should the pressed state force normal state transition on the hover target.")]
	private bool m_PressForceHoverNormal = true;

	private bool isPointerDown;

	private bool isPointerInside;

	private bool m_DragHasBegan;

	private bool m_DropPreformed;

	private bool m_IsTooltipShown;

	public bool dragAndDropEnabled
	{
		get
		{
			return m_DragAndDropEnabled;
		}
		set
		{
			m_DragAndDropEnabled = value;
		}
	}

	public bool isStatic
	{
		get
		{
			return m_IsStatic;
		}
		set
		{
			m_IsStatic = value;
		}
	}

	public bool allowThrowAway
	{
		get
		{
			return m_AllowThrowAway;
		}
		set
		{
			m_AllowThrowAway = value;
		}
	}

	public DragKeyModifier dragKeyModifier
	{
		get
		{
			return m_DragKeyModifier;
		}
		set
		{
			m_DragKeyModifier = value;
		}
	}

	public bool tooltipEnabled
	{
		get
		{
			return m_TooltipEnabled;
		}
		set
		{
			m_TooltipEnabled = value;
		}
	}

	public float tooltipDelay
	{
		get
		{
			return m_TooltipDelay;
		}
		set
		{
			m_TooltipDelay = value;
		}
	}

	public bool pressTransitionInstaOut
	{
		get
		{
			return m_PressTransitionInstaOut;
		}
		set
		{
			m_PressTransitionInstaOut = value;
		}
	}

	public bool pressForceHoverNormal
	{
		get
		{
			return m_PressForceHoverNormal;
		}
		set
		{
			m_PressForceHoverNormal = value;
		}
	}

	public bool dropPreformed
	{
		get
		{
			return m_DropPreformed;
		}
		set
		{
			m_DropPreformed = value;
		}
	}

	protected override void Start()
	{
		if (!IsAssigned() && iconGraphic != null && iconGraphic.gameObject.activeSelf)
		{
			iconGraphic.gameObject.SetActive(value: false);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		EvaluateAndTransitionHoveredState(instant: true);
		EvaluateAndTransitionPressedState(instant: true);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		isPointerInside = false;
		isPointerDown = false;
		EvaluateAndTransitionHoveredState(instant: true);
		EvaluateAndTransitionPressedState(instant: true);
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		isPointerInside = true;
		EvaluateAndTransitionHoveredState(instant: false);
		if (base.enabled && IsActive() && m_TooltipEnabled)
		{
			if (m_TooltipDelay > 0f)
			{
				StartCoroutine("TooltipDelayedShow");
			}
			else
			{
				InternalShowTooltip();
			}
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		isPointerInside = false;
		EvaluateAndTransitionHoveredState(instant: false);
		InternalHideTooltip();
	}

	public virtual void OnTooltip(bool show)
	{
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		isPointerDown = true;
		EvaluateAndTransitionPressedState(instant: false);
		InternalHideTooltip();
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown = false;
		EvaluateAndTransitionPressedState(m_PressTransitionInstaOut);
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
	}

	protected bool IsHighlighted(BaseEventData eventData)
	{
		if (!IsActive())
		{
			return false;
		}
		if (eventData is PointerEventData)
		{
			PointerEventData pointerEventData = eventData as PointerEventData;
			if ((!isPointerDown || isPointerInside || !(pointerEventData.pointerPress == base.gameObject)) && (isPointerDown || !isPointerInside || !(pointerEventData.pointerPress == base.gameObject)))
			{
				if (!isPointerDown && isPointerInside)
				{
					return pointerEventData.pointerPress == null;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	protected bool IsPressed(BaseEventData eventData)
	{
		if (IsActive() && isPointerInside)
		{
			return isPointerDown;
		}
		return false;
	}

	protected virtual void EvaluateAndTransitionHoveredState(bool instant)
	{
		if (IsActive() && !(hoverTargetGraphic == null) && hoverTargetGraphic.gameObject.activeInHierarchy)
		{
			bool highlighted = ((!m_PressForceHoverNormal) ? isPointerInside : (isPointerInside && !isPointerDown));
			switch (hoverTransition)
			{
			case Transition.ColorTint:
				StartColorTween(hoverTargetGraphic, highlighted ? hoverHighlightColor : hoverNormalColor, instant ? 0f : hoverTransitionDuration);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(hoverTargetGraphic, highlighted ? hoverOverrideSprite : null);
				break;
			case Transition.Animation:
				TriggerHoverStateAnimation(highlighted ? hoverHighlightTrigger : hoverNormalTrigger);
				break;
			}
		}
	}

	protected virtual void EvaluateAndTransitionPressedState(bool instant)
	{
		if (IsActive() && !(pressTargetGraphic == null) && pressTargetGraphic.gameObject.activeInHierarchy)
		{
			switch (pressTransition)
			{
			case Transition.ColorTint:
				StartColorTween(pressTargetGraphic, isPointerDown ? pressPressColor : pressNormalColor, instant ? 0f : pressTransitionDuration);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(pressTargetGraphic, isPointerDown ? pressOverrideSprite : null);
				break;
			case Transition.Animation:
				TriggerPressStateAnimation(isPointerDown ? pressPressTrigger : pressNormalTrigger);
				break;
			}
			if (m_PressForceHoverNormal)
			{
				EvaluateAndTransitionHoveredState(instant: false);
			}
		}
	}

	protected virtual void StartColorTween(Graphic target, Color targetColor, float duration)
	{
		if (!(target == null))
		{
			target.CrossFadeColor(targetColor, duration, ignoreTimeScale: true, useAlpha: true);
		}
	}

	protected virtual void DoSpriteSwap(Graphic target, Sprite newSprite)
	{
		if (!(target == null))
		{
			Image image = target as Image;
			if (!(image == null))
			{
				image.overrideSprite = newSprite;
			}
		}
	}

	private void TriggerHoverStateAnimation(string triggername)
	{
		if (!(hoverTargetGraphic == null))
		{
			Animator animator = hoverTargetGraphic.gameObject.GetComponent<Animator>();
			if (!(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
			{
				animator.ResetTrigger(hoverNormalTrigger);
				animator.ResetTrigger(hoverHighlightTrigger);
				animator.SetTrigger(triggername);
			}
		}
	}

	private void TriggerPressStateAnimation(string triggername)
	{
		if (!(pressTargetGraphic == null))
		{
			Animator animator = pressTargetGraphic.gameObject.GetComponent<Animator>();
			if (!(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
			{
				animator.ResetTrigger(pressNormalTrigger);
				animator.ResetTrigger(pressPressTrigger);
				animator.SetTrigger(triggername);
			}
		}
	}

	public virtual bool IsAssigned()
	{
		if (!(GetIconSprite() != null))
		{
			return GetIconTexture() != null;
		}
		return true;
	}

	public bool Assign(Sprite icon)
	{
		if (icon == null)
		{
			return false;
		}
		SetIcon(icon);
		return true;
	}

	public bool Assign(Texture icon)
	{
		if (icon == null)
		{
			return false;
		}
		SetIcon(icon);
		return true;
	}

	public virtual bool Assign(Object source)
	{
		if (source is UISlotBase)
		{
			UISlotBase sourceSlot = source as UISlotBase;
			if (sourceSlot != null)
			{
				if (sourceSlot.GetIconSprite() != null)
				{
					return Assign(sourceSlot.GetIconSprite());
				}
				if (sourceSlot.GetIconTexture() != null)
				{
					return Assign(sourceSlot.GetIconTexture());
				}
			}
		}
		return false;
	}

	public virtual void Unassign()
	{
		ClearIcon();
	}

	public Sprite GetIconSprite()
	{
		if (iconGraphic == null || !(iconGraphic is Image))
		{
			return null;
		}
		return (iconGraphic as Image).sprite;
	}

	public Texture GetIconTexture()
	{
		if (iconGraphic == null || !(iconGraphic is RawImage))
		{
			return null;
		}
		return (iconGraphic as RawImage).texture;
	}

	public Object GetIconAsObject()
	{
		if (iconGraphic == null)
		{
			return null;
		}
		if (iconGraphic is Image)
		{
			return GetIconSprite();
		}
		if (iconGraphic is RawImage)
		{
			return GetIconTexture();
		}
		return null;
	}

	public void SetIcon(Sprite iconSprite)
	{
		if (!(iconGraphic == null) && iconGraphic is Image)
		{
			(iconGraphic as Image).sprite = iconSprite;
			if (iconSprite != null && !iconGraphic.gameObject.activeSelf)
			{
				iconGraphic.gameObject.SetActive(value: true);
			}
			if (iconSprite == null && iconGraphic.gameObject.activeSelf)
			{
				iconGraphic.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetIcon(Texture iconTex)
	{
		if (!(iconGraphic == null) && iconGraphic is RawImage)
		{
			(iconGraphic as RawImage).texture = iconTex;
			if (iconTex != null && !iconGraphic.gameObject.activeSelf)
			{
				iconGraphic.gameObject.SetActive(value: true);
			}
			if (iconTex == null && iconGraphic.gameObject.activeSelf)
			{
				iconGraphic.gameObject.SetActive(value: false);
			}
		}
	}

	public void ClearIcon()
	{
		if (!(iconGraphic == null))
		{
			if (iconGraphic is Image)
			{
				(iconGraphic as Image).sprite = null;
			}
			if (iconGraphic is RawImage)
			{
				(iconGraphic as RawImage).texture = null;
			}
			iconGraphic.gameObject.SetActive(value: false);
		}
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		if (!base.enabled || !IsAssigned() || !m_DragAndDropEnabled)
		{
			eventData.Reset();
			return;
		}
		if (!DragKeyModifierIsDown())
		{
			eventData.Reset();
			return;
		}
		m_DragHasBegan = true;
		CreateTemporaryIcon(eventData);
		eventData.Use();
	}

	public virtual bool DragKeyModifierIsDown()
	{
		switch (m_DragKeyModifier)
		{
		case DragKeyModifier.Control:
			if (!Input.GetKey(KeyCode.LeftControl))
			{
				return Input.GetKey(KeyCode.RightControl);
			}
			return true;
		case DragKeyModifier.Alt:
			if (!Input.GetKey(KeyCode.LeftAlt))
			{
				return Input.GetKey(KeyCode.RightAlt);
			}
			return true;
		case DragKeyModifier.Shift:
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		default:
			return true;
		}
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		if (m_DragHasBegan && m_CurrentDraggedObject != null)
		{
			UpdateDraggedPosition(eventData);
		}
	}

	public virtual void OnDrop(PointerEventData eventData)
	{
		UISlotBase source = ((eventData.pointerPress != null) ? eventData.pointerPress.GetComponent<UISlotBase>() : null);
		if (source == null || !source.IsAssigned() || !source.dragAndDropEnabled)
		{
			return;
		}
		source.dropPreformed = true;
		if (!base.enabled || !m_DragAndDropEnabled)
		{
			return;
		}
		bool assignSuccess = false;
		if (!IsAssigned())
		{
			assignSuccess = Assign(source);
			if (assignSuccess && !source.isStatic)
			{
				source.Unassign();
			}
		}
		else if (!isStatic && !source.isStatic)
		{
			if (CanSwapWith(source) && source.CanSwapWith(this))
			{
				assignSuccess = source.PerformSlotSwap(this);
			}
		}
		else if (!isStatic && source.isStatic)
		{
			assignSuccess = Assign(source);
		}
		if (!assignSuccess)
		{
			OnAssignBySlotFailed(source);
		}
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		if (!m_DragHasBegan)
		{
			return;
		}
		m_DragHasBegan = false;
		if (m_CurrentDraggedObject != null)
		{
			Object.Destroy(m_CurrentDraggedObject);
		}
		m_CurrentDraggedObject = null;
		m_CurrentDraggingPlane = null;
		if (!IsHighlighted(eventData))
		{
			if (!m_DropPreformed)
			{
				OnThrowAway();
			}
			else
			{
				m_DropPreformed = false;
			}
		}
	}

	public virtual bool CanSwapWith(Object target)
	{
		return target is UISlotBase;
	}

	public virtual bool PerformSlotSwap(Object targetObject)
	{
		UISlotBase obj = targetObject as UISlotBase;
		Object targetIcon = obj.GetIconAsObject();
		bool num = obj.Assign(this);
		bool assign2 = Assign(targetIcon);
		return num && assign2;
	}

	protected virtual void OnAssignBySlotFailed(Object source)
	{
		Debug.Log("UISlotBase (" + base.gameObject.name + ") failed to get assigned by (" + (source as UISlotBase).gameObject.name + ").");
	}

	protected virtual void OnThrowAway()
	{
		if (m_AllowThrowAway)
		{
			Unassign();
		}
		else
		{
			OnThrowAwayDenied();
		}
	}

	protected virtual void OnThrowAwayDenied()
	{
	}

	protected virtual void CreateTemporaryIcon(PointerEventData eventData)
	{
		Canvas canvas = UIUtility.FindInParents<Canvas>(base.gameObject);
		if (!(canvas == null) && !(iconGraphic == null))
		{
			GameObject iconObj = Object.Instantiate((m_CloneTarget == null) ? iconGraphic.gameObject : m_CloneTarget);
			iconObj.transform.localScale = new Vector3(1f, 1f, 1f);
			iconObj.transform.SetParent(canvas.transform, worldPositionStays: false);
			iconObj.transform.SetAsLastSibling();
			(iconObj.transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
			iconObj.AddComponent<UIIgnoreRaycast>();
			m_CurrentDraggingPlane = canvas.transform as RectTransform;
			m_CurrentDraggedObject = iconObj;
			UpdateDraggedPosition(eventData);
		}
	}

	private void UpdateDraggedPosition(PointerEventData data)
	{
		RectTransform rt = m_CurrentDraggedObject.GetComponent<RectTransform>();
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_CurrentDraggingPlane, data.position, data.pressEventCamera, out var globalMousePos))
		{
			rt.position = globalMousePos;
			rt.rotation = m_CurrentDraggingPlane.rotation;
		}
	}

	protected void InternalShowTooltip()
	{
		if (!m_IsTooltipShown)
		{
			m_IsTooltipShown = true;
			OnTooltip(show: true);
		}
	}

	protected void InternalHideTooltip()
	{
		StopCoroutine("TooltipDelayedShow");
		if (m_IsTooltipShown)
		{
			m_IsTooltipShown = false;
			OnTooltip(show: false);
		}
	}

	protected IEnumerator TooltipDelayedShow()
	{
		yield return new WaitForSeconds(m_TooltipDelay);
		InternalShowTooltip();
	}
}
