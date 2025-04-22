using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

public class UISelectField_Option : Toggle, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public class SelectOptionEvent : UnityEvent<string>
	{
	}

	[Serializable]
	public class PointerUpEvent : UnityEvent<BaseEventData>
	{
	}

	public UISelectField selectField;

	public Text textComponent;

	public SelectOptionEvent onSelectOption = new SelectOptionEvent();

	public PointerUpEvent onPointerUp = new PointerUpEvent();

	protected override void Start()
	{
		base.Start();
		Navigation nav = default(Navigation);
		nav.mode = Navigation.Mode.None;
		base.navigation = nav;
		base.transition = Transition.None;
		toggleTransition = ToggleTransition.None;
	}

	public void Initialize(UISelectField select, Text text)
	{
		selectField = select;
		textComponent = text;
		OnEnable();
	}

	public new bool IsPressed()
	{
		return base.IsPressed();
	}

	public bool IsHighlighted(BaseEventData eventData)
	{
		return IsHighlighted();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (onPointerUp != null)
		{
			onPointerUp.Invoke(eventData);
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		DoStateTransition(SelectionState.Normal, instant: false);
		if (onSelectOption != null && textComponent != null)
		{
			onSelectOption.Invoke(textComponent.text);
		}
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		if (base.enabled && base.enabled && base.gameObject.activeInHierarchy && !(selectField == null))
		{
			Color color = selectField.optionBackgroundTransColors.normalColor;
			Sprite newSprite = null;
			string triggername = selectField.optionBackgroundAnimationTriggers.normalTrigger;
			switch (state)
			{
			case SelectionState.Disabled:
				color = selectField.optionBackgroundTransColors.disabledColor;
				newSprite = selectField.optionBackgroundSpriteStates.disabledSprite;
				triggername = selectField.optionBackgroundAnimationTriggers.disabledTrigger;
				break;
			case SelectionState.Normal:
				color = (base.isOn ? selectField.optionBackgroundTransColors.activeColor : selectField.optionBackgroundTransColors.normalColor);
				newSprite = (base.isOn ? selectField.optionBackgroundSpriteStates.activeSprite : null);
				triggername = (base.isOn ? selectField.optionBackgroundAnimationTriggers.activeTrigger : selectField.optionBackgroundAnimationTriggers.normalTrigger);
				break;
			case SelectionState.Highlighted:
				color = (base.isOn ? selectField.optionBackgroundTransColors.activeHighlightedColor : selectField.optionBackgroundTransColors.highlightedColor);
				newSprite = (base.isOn ? selectField.optionBackgroundSpriteStates.activeHighlightedSprite : selectField.optionBackgroundSpriteStates.highlightedSprite);
				triggername = (base.isOn ? selectField.optionBackgroundAnimationTriggers.activeHighlightedTrigger : selectField.optionBackgroundAnimationTriggers.highlightedTrigger);
				break;
			case SelectionState.Pressed:
				color = (base.isOn ? selectField.optionBackgroundTransColors.activePressedColor : selectField.optionBackgroundTransColors.pressedColor);
				newSprite = (base.isOn ? selectField.optionBackgroundSpriteStates.activePressedSprite : selectField.optionBackgroundSpriteStates.pressedSprite);
				triggername = (base.isOn ? selectField.optionBackgroundAnimationTriggers.activePressedTrigger : selectField.optionBackgroundAnimationTriggers.pressedTrigger);
				break;
			}
			switch (selectField.optionBackgroundTransitionType)
			{
			case Transition.ColorTint:
				StartColorTween(color * selectField.optionBackgroundTransColors.colorMultiplier, instant ? 0f : selectField.optionBackgroundTransColors.fadeDuration);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(newSprite);
				break;
			case Transition.Animation:
				TriggerAnimation(triggername);
				break;
			}
			DoTextStateTransition(state, instant);
		}
	}

	private void DoTextStateTransition(SelectionState state, bool instant)
	{
		if (selectField != null && textComponent != null && selectField.optionTextTransitionType == UISelectField.OptionTextTransitionType.CrossFade)
		{
			Color color = selectField.optionTextTransitionColors.normalColor;
			switch (state)
			{
			case SelectionState.Disabled:
				color = selectField.optionTextTransitionColors.disabledColor;
				break;
			case SelectionState.Normal:
				color = (base.isOn ? selectField.optionTextTransitionColors.activeColor : selectField.optionTextTransitionColors.normalColor);
				break;
			case SelectionState.Highlighted:
				color = (base.isOn ? selectField.optionTextTransitionColors.activeHighlightedColor : selectField.optionTextTransitionColors.highlightedColor);
				break;
			case SelectionState.Pressed:
				color = (base.isOn ? selectField.optionTextTransitionColors.activePressedColor : selectField.optionTextTransitionColors.pressedColor);
				break;
			}
			textComponent.CrossFadeColor(color * selectField.optionTextTransitionColors.colorMultiplier, instant ? 0f : selectField.optionTextTransitionColors.fadeDuration, ignoreTimeScale: true, useAlpha: true);
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
		if (!(selectField == null) && !(base.animator == null) && base.animator.enabled && base.animator.isActiveAndEnabled && !(base.animator.runtimeAnimatorController == null) && base.animator.hasBoundPlayables && !string.IsNullOrEmpty(trigger))
		{
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.normalTrigger);
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.pressedTrigger);
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.highlightedTrigger);
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.activeTrigger);
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.activeHighlightedTrigger);
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.activePressedTrigger);
			base.animator.ResetTrigger(selectField.optionBackgroundAnimationTriggers.disabledTrigger);
			base.animator.SetTrigger(trigger);
		}
	}
}
