using System;
using DG.Tweening;
using UnityEngine;

[LogicUpdatePriority(80)]
public class UI_InGame_SkillButtons : ManagerBase<UI_InGame_SkillButtons>, IGamepadFocusable, IGamepadFocusListener, IGamepadFocusableOverrideInput, IGamepadFocusableOverrideTooltip, IGamepadPingProxyParent
{
	public float selfExpandedScale = 1.5f;

	public float layoutExpandedScale = 1.35f;

	public Transform[] expandedLayouts;

	public float animDuration = 0.3f;

	public float hiddenWhenExpandedDuration = 0.1f;

	public CanvasGroup[] hiddenWhenExpanded;

	public Transform[] adjustedItems;

	public Transform[] targetOnKeyboardMouse;

	public Transform[] targetOnGamepad;

	public UI_InGame_SkillButton[] skillButtons;

	private float _selfDefaultScale;

	private Vector3[] _cvs;

	private Canvas _canvas;

	private bool _isAnimating;

	public MonoBehaviour pingableTarget
	{
		get
		{
			if (ManagerBase<EditSkillManager>.instance.selectedSkillSlot.HasValue)
			{
				return skillButtons[(int)ManagerBase<EditSkillManager>.instance.selectedSkillSlot.Value];
			}
			if (ManagerBase<EditSkillManager>.instance.selectedGemSlot.HasValue)
			{
				GemLocation value = ManagerBase<EditSkillManager>.instance.selectedGemSlot.Value;
				return skillButtons[(int)value.skill].GetGemSlot(value.index);
			}
			return null;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		layoutExpandedScale = 1.9f;
		_canvas = GetComponent<Canvas>();
		_selfDefaultScale = base.transform.localScale.x;
		_cvs = new Vector3[adjustedItems.Length];
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		Transform[] array = ((DewInput.currentMode == InputMode.KeyboardAndMouse) ? targetOnKeyboardMouse : targetOnGamepad);
		for (int i = 0; i < adjustedItems.Length; i++)
		{
			adjustedItems[i].position = Vector3.SmoothDamp(adjustedItems[i].position, array[i].position, ref _cvs[i], 0.075f);
		}
	}

	private void Start()
	{
		EditSkillManager editSkillManager = ManagerBase<EditSkillManager>.instance;
		editSkillManager.onModeChanged = (Action<EditSkillManager.ModeType>)Delegate.Combine(editSkillManager.onModeChanged, new Action<EditSkillManager.ModeType>(OnStateChanged));
		CameraManager cameraManager = ManagerBase<CameraManager>.instance;
		cameraManager.onIsSpectatingChanged = (Action<bool>)Delegate.Combine(cameraManager.onIsSpectatingChanged, new Action<bool>(OnIsSpectatingChanged));
		ManagerBase<GlobalUIManager>.instance.AddGamepadFocusable(this);
	}

	private void OnDestroy()
	{
		if (ManagerBase<GlobalUIManager>.instance != null)
		{
			ManagerBase<GlobalUIManager>.instance.RemoveGamepadFocusable(this);
		}
	}

	private void OnIsSpectatingChanged(bool obj)
	{
		_canvas.enabled = !obj;
	}

	private void OnStateChanged(EditSkillManager.ModeType mode)
	{
		if (mode != 0)
		{
			base.transform.DOScale(Vector3.one * selfExpandedScale, animDuration).SetUpdate(isIndependentUpdate: true);
			for (int i = 0; i < expandedLayouts.Length; i++)
			{
				expandedLayouts[i].localScale = Vector3.one * layoutExpandedScale;
			}
			CanvasGroup[] array = hiddenWhenExpanded;
			if (_isAnimating)
			{
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				RectTransform component = array[j].GetComponent<RectTransform>();
				if (component != null)
				{
					component.DOKill(complete: true);
					component.DOAnchorPosY(component.anchoredPosition.y + component.parent.GetComponent<RectTransform>().rect.height * 2f, hiddenWhenExpandedDuration).SetUpdate(isIndependentUpdate: true);
				}
			}
			_isAnimating = true;
		}
		else
		{
			base.transform.DOScale(Vector3.one * _selfDefaultScale, animDuration).SetUpdate(isIndependentUpdate: true);
			for (int k = 0; k < expandedLayouts.Length; k++)
			{
				expandedLayouts[k].localScale = Vector3.one;
			}
			CanvasGroup[] array2 = hiddenWhenExpanded;
			for (int l = 0; l < array2.Length; l++)
			{
				RectTransform component2 = array2[l].GetComponent<RectTransform>();
				if (component2 != null && _isAnimating)
				{
					component2.DOKill(complete: true);
					component2.DOAnchorPosY(component2.anchoredPosition.y - component2.parent.GetComponent<RectTransform>().rect.height * 2f, hiddenWhenExpandedDuration).SetUpdate(isIndependentUpdate: true);
				}
			}
			_isAnimating = false;
		}
		if (DewInput.currentMode == InputMode.Gamepad)
		{
			if (mode == EditSkillManager.ModeType.None && ManagerBase<GlobalUIManager>.instance.focused == this)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(null);
			}
			else if (mode != 0 && ManagerBase<GlobalUIManager>.instance.focused == null)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(this);
			}
		}
	}

	public void OnFocusStateChanged(bool state)
	{
		if (!ManagerBase<ControlManager>.instance.isEditSkillDisabled)
		{
			if (state && ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.None)
			{
				ManagerBase<EditSkillManager>.instance.StartRegularEdit(endAfterAction: true);
			}
			else if (!state && (ManagerBase<EditSkillManager>.instance.mode == EditSkillManager.ModeType.Regular || ManagerBase<GlobalUIManager>.instance.focused == null))
			{
				ManagerBase<EditSkillManager>.instance.EndEdit();
			}
		}
		if (state)
		{
			ManagerBase<EditSkillManager>.instance.SelectAnyRelevantSlot();
		}
		else
		{
			ManagerBase<EditSkillManager>.instance.ClearGamepadSelection();
		}
	}

	public FocusableBehavior GetBehavior()
	{
		return FocusableBehavior.BottomBar;
	}

	public bool CanBeFocused()
	{
		if (ManagerBase<ControlManager>.instance.shouldProcessCharacterInput)
		{
			if (ManagerBase<ControlManager>.instance.isEditSkillDisabled)
			{
				return ManagerBase<EditSkillManager>.instance.mode > EditSkillManager.ModeType.None;
			}
			return true;
		}
		return false;
	}

	public SelectionDisplayType GetSelectionDisplayType()
	{
		return SelectionDisplayType.Dont;
	}

	public bool OnGamepadDpadUp()
	{
		if (ManagerBase<EditSkillManager>.instance.draggingObject != null && ManagerBase<EditSkillManager>.instance.selectedSkillSlot.HasValue)
		{
			ManagerBase<EditSkillManager>.instance.SelectGround();
			SingletonBehaviour<UI_TooltipManager>.instance.Hide();
			return true;
		}
		return ManagerBase<EditSkillManager>.instance.DoDpadUp();
	}

	public bool OnGamepadDpadLeft()
	{
		return ManagerBase<EditSkillManager>.instance.DoDpadLeft();
	}

	public bool OnGamepadDpadRight()
	{
		return ManagerBase<EditSkillManager>.instance.DoDpadRight();
	}

	public bool OnGamepadDpadDown()
	{
		if (ManagerBase<EditSkillManager>.instance.isSelectingGround)
		{
			ManagerBase<EditSkillManager>.instance.UnselectGround();
		}
		return ManagerBase<EditSkillManager>.instance.DoDpadDown();
	}

	public bool OnGamepadConfirm()
	{
		ManagerBase<EditSkillManager>.instance.DoConfirm();
		Dew.CallDelayed(delegate
		{
			if (ManagerBase<EditSkillManager>.instance.selectedSkillSlot.HasValue)
			{
				skillButtons[(int)ManagerBase<EditSkillManager>.instance.selectedSkillSlot.Value].ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
			}
			else if (ManagerBase<EditSkillManager>.instance.selectedGemSlot.HasValue)
			{
				GemLocation value = ManagerBase<EditSkillManager>.instance.selectedGemSlot.Value;
				skillButtons[(int)value.skill].GetGemSlot(value.index).ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
			}
		}, 2);
		return true;
	}

	public bool OnGamepadBack()
	{
		if (ManagerBase<EditSkillManager>.instance.isSelectingGround)
		{
			ManagerBase<EditSkillManager>.instance.UnselectGround();
			return true;
		}
		if (ManagerBase<EditSkillManager>.instance.isDragging)
		{
			ManagerBase<EditSkillManager>.instance.EndDrag(isCancel: true);
			return true;
		}
		return ManagerBase<EditSkillManager>.instance.DoBack();
	}

	public bool OnUpdateTooltip()
	{
		if (ManagerBase<EditSkillManager>.instance.selectedSkillSlot.HasValue)
		{
			skillButtons[(int)ManagerBase<EditSkillManager>.instance.selectedSkillSlot.Value].ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
		}
		else if (ManagerBase<EditSkillManager>.instance.selectedGemSlot.HasValue)
		{
			GemLocation value = ManagerBase<EditSkillManager>.instance.selectedGemSlot.Value;
			skillButtons[(int)value.skill].GetGemSlot(value.index).ShowTooltip(SingletonBehaviour<UI_TooltipManager>.instance);
		}
		return true;
	}
}
