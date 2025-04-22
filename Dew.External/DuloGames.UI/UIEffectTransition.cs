using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[AddComponentMenu("UI/Effect Transition")]
public class UIEffectTransition : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum VisualState
	{
		Normal,
		Highlighted,
		Selected,
		Pressed,
		Active
	}

	[SerializeField]
	[Tooltip("Graphic that will have the selected transtion applied.")]
	private BaseMeshEffect m_TargetEffect;

	[SerializeField]
	private Color m_NormalColor = ColorBlock.defaultColorBlock.normalColor;

	[SerializeField]
	private Color m_HighlightedColor = ColorBlock.defaultColorBlock.highlightedColor;

	[SerializeField]
	private Color m_SelectedColor = ColorBlock.defaultColorBlock.highlightedColor;

	[SerializeField]
	private Color m_PressedColor = ColorBlock.defaultColorBlock.pressedColor;

	[SerializeField]
	private float m_Duration = 0.1f;

	[SerializeField]
	private bool m_UseToggle;

	[SerializeField]
	private Toggle m_TargetToggle;

	[SerializeField]
	private Color m_ActiveColor = ColorBlock.defaultColorBlock.highlightedColor;

	private bool m_Highlighted;

	private bool m_Selected;

	private bool m_Pressed;

	private bool m_Active;

	private Selectable m_Selectable;

	private bool m_GroupsAllowInteraction = true;

	[NonSerialized]
	private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

	private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

	protected UIEffectTransition()
	{
		if (m_ColorTweenRunner == null)
		{
			m_ColorTweenRunner = new TweenRunner<ColorTween>();
		}
		m_ColorTweenRunner.Init(this);
	}

	protected void Awake()
	{
		if (m_UseToggle)
		{
			if (m_TargetToggle == null)
			{
				m_TargetToggle = base.gameObject.GetComponent<Toggle>();
			}
			if (m_TargetToggle != null)
			{
				m_Active = m_TargetToggle.isOn;
			}
		}
		m_Selectable = base.gameObject.GetComponent<Selectable>();
	}

	protected void OnEnable()
	{
		if (m_TargetToggle != null)
		{
			m_TargetToggle.onValueChanged.AddListener(OnToggleValueChange);
		}
		InternalEvaluateAndTransitionToNormalState(instant: true);
	}

	protected void OnDisable()
	{
		if (m_TargetToggle != null)
		{
			m_TargetToggle.onValueChanged.RemoveListener(OnToggleValueChange);
		}
		InstantClearState();
	}

	protected void OnCanvasGroupChanged()
	{
		bool groupAllowInteraction = true;
		Transform t = base.transform;
		while (t != null)
		{
			t.GetComponents(m_CanvasGroupCache);
			bool shouldBreak = false;
			for (int i = 0; i < m_CanvasGroupCache.Count; i++)
			{
				if (!m_CanvasGroupCache[i].interactable)
				{
					groupAllowInteraction = false;
					shouldBreak = true;
				}
				if (m_CanvasGroupCache[i].ignoreParentGroups)
				{
					shouldBreak = true;
				}
			}
			if (shouldBreak)
			{
				break;
			}
			t = t.parent;
		}
		if (groupAllowInteraction != m_GroupsAllowInteraction)
		{
			m_GroupsAllowInteraction = groupAllowInteraction;
			InternalEvaluateAndTransitionToNormalState(instant: true);
		}
	}

	public virtual bool IsInteractable()
	{
		if (m_Selectable != null)
		{
			if (m_Selectable.IsInteractable())
			{
				return m_GroupsAllowInteraction;
			}
			return false;
		}
		return m_GroupsAllowInteraction;
	}

	protected void OnToggleValueChange(bool value)
	{
		if (m_UseToggle && !(m_TargetToggle == null))
		{
			m_Active = m_TargetToggle.isOn;
			if (!m_TargetToggle.isOn)
			{
				DoStateTransition(m_Selected ? VisualState.Selected : VisualState.Normal, instant: false);
			}
		}
	}

	protected void InstantClearState()
	{
		SetEffectColor(Color.white);
	}

	private void InternalEvaluateAndTransitionToNormalState(bool instant)
	{
		DoStateTransition(m_Active ? VisualState.Active : VisualState.Normal, instant);
	}

	public void OnSelect(BaseEventData eventData)
	{
		m_Selected = true;
		if (!m_Active)
		{
			DoStateTransition(VisualState.Selected, instant: false);
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		m_Selected = false;
		if (!m_Active)
		{
			DoStateTransition(m_Highlighted ? VisualState.Highlighted : VisualState.Normal, instant: false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		m_Highlighted = true;
		if (!m_Selected && !m_Pressed && !m_Active)
		{
			DoStateTransition(VisualState.Highlighted, instant: false);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_Highlighted = false;
		if (!m_Selected && !m_Pressed && !m_Active)
		{
			DoStateTransition(VisualState.Normal, instant: false);
		}
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && m_Highlighted)
		{
			m_Pressed = true;
			DoStateTransition(VisualState.Pressed, instant: false);
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			m_Pressed = false;
			VisualState newState = VisualState.Normal;
			if (m_Active)
			{
				newState = VisualState.Active;
			}
			else if (m_Selected)
			{
				newState = VisualState.Selected;
			}
			else if (m_Highlighted)
			{
				newState = VisualState.Highlighted;
			}
			DoStateTransition(newState, instant: false);
		}
	}

	protected virtual void DoStateTransition(VisualState state, bool instant)
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (!IsInteractable())
			{
				state = VisualState.Normal;
			}
			Color color = m_NormalColor;
			switch (state)
			{
			case VisualState.Normal:
				color = m_NormalColor;
				break;
			case VisualState.Highlighted:
				color = m_HighlightedColor;
				break;
			case VisualState.Selected:
				color = m_SelectedColor;
				break;
			case VisualState.Pressed:
				color = m_PressedColor;
				break;
			case VisualState.Active:
				color = m_ActiveColor;
				break;
			}
			StartEffectColorTween(color, instant: false);
		}
	}

	private void StartEffectColorTween(Color targetColor, bool instant)
	{
		if (!(m_TargetEffect == null) && (m_TargetEffect is Shadow || m_TargetEffect is Outline))
		{
			if (instant || m_Duration == 0f || !Application.isPlaying)
			{
				SetEffectColor(targetColor);
				return;
			}
			ColorTween colorTween = default(ColorTween);
			colorTween.duration = m_Duration;
			colorTween.startColor = GetEffectColor();
			colorTween.targetColor = targetColor;
			ColorTween colorTween2 = colorTween;
			colorTween2.AddOnChangedCallback(SetEffectColor);
			colorTween2.ignoreTimeScale = true;
			m_ColorTweenRunner.StartTween(colorTween2);
		}
	}

	private void SetEffectColor(Color targetColor)
	{
		if (!(m_TargetEffect == null))
		{
			if (m_TargetEffect is Shadow)
			{
				(m_TargetEffect as Shadow).effectColor = targetColor;
			}
			else if (m_TargetEffect is Outline)
			{
				(m_TargetEffect as Outline).effectColor = targetColor;
			}
		}
	}

	private Color GetEffectColor()
	{
		if (m_TargetEffect == null)
		{
			return Color.white;
		}
		if (m_TargetEffect is Shadow)
		{
			return (m_TargetEffect as Shadow).effectColor;
		}
		if (m_TargetEffect is Outline)
		{
			return (m_TargetEffect as Outline).effectColor;
		}
		return Color.white;
	}
}
