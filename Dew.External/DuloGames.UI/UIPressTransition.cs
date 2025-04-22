using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteAlways]
[AddComponentMenu("UI/Press Transition")]
public class UIPressTransition : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum VisualState
	{
		Normal,
		Pressed
	}

	public enum Transition
	{
		None,
		ColorTint,
		SpriteSwap,
		Animation,
		TextColor
	}

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private Color m_NormalColor = ColorBlock.defaultColorBlock.normalColor;

	[SerializeField]
	private Color m_PressedColor = ColorBlock.defaultColorBlock.pressedColor;

	[SerializeField]
	private float m_Duration = 0.1f;

	[SerializeField]
	[Range(1f, 6f)]
	private float m_ColorMultiplier = 1f;

	[SerializeField]
	private Sprite m_PressedSprite;

	[SerializeField]
	private string m_NormalTrigger = "Normal";

	[SerializeField]
	private string m_PressedTrigger = "Pressed";

	[SerializeField]
	[Tooltip("Graphic that will have the selected transtion applied.")]
	private Graphic m_TargetGraphic;

	[SerializeField]
	[Tooltip("GameObject that will have the selected transtion applied.")]
	private GameObject m_TargetGameObject;

	private Selectable m_Selectable;

	private bool m_GroupsAllowInteraction = true;

	[NonSerialized]
	private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

	private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

	public Transition transition
	{
		get
		{
			return m_Transition;
		}
		set
		{
			m_Transition = value;
		}
	}

	public Graphic targetGraphic
	{
		get
		{
			return m_TargetGraphic;
		}
		set
		{
			m_TargetGraphic = value;
		}
	}

	public GameObject targetGameObject
	{
		get
		{
			return m_TargetGameObject;
		}
		set
		{
			m_TargetGameObject = value;
		}
	}

	public Animator animator
	{
		get
		{
			if (m_TargetGameObject != null)
			{
				return m_TargetGameObject.GetComponent<Animator>();
			}
			return null;
		}
	}

	protected UIPressTransition()
	{
		if (m_ColorTweenRunner == null)
		{
			m_ColorTweenRunner = new TweenRunner<ColorTween>();
		}
		m_ColorTweenRunner.Init(this);
	}

	protected void Awake()
	{
		m_Selectable = base.gameObject.GetComponent<Selectable>();
	}

	protected void OnEnable()
	{
		InternalEvaluateAndTransitionToNormalState(instant: true);
	}

	protected void OnDisable()
	{
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

	protected void InstantClearState()
	{
		switch (m_Transition)
		{
		case Transition.ColorTint:
			StartColorTween(Color.white, instant: true);
			break;
		case Transition.SpriteSwap:
			DoSpriteSwap(null);
			break;
		case Transition.TextColor:
			SetTextColor(Color.white);
			break;
		case Transition.Animation:
			break;
		}
	}

	private void InternalEvaluateAndTransitionToNormalState(bool instant)
	{
		DoStateTransition(VisualState.Normal, instant);
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		DoStateTransition(VisualState.Pressed, instant: false);
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		DoStateTransition(VisualState.Normal, instant: false);
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
			Sprite newSprite = null;
			string triggername = m_NormalTrigger;
			switch (state)
			{
			case VisualState.Normal:
				color = m_NormalColor;
				newSprite = null;
				triggername = m_NormalTrigger;
				break;
			case VisualState.Pressed:
				color = m_PressedColor;
				newSprite = m_PressedSprite;
				triggername = m_PressedTrigger;
				break;
			}
			switch (m_Transition)
			{
			case Transition.ColorTint:
				StartColorTween(color * m_ColorMultiplier, instant);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(newSprite);
				break;
			case Transition.Animation:
				TriggerAnimation(triggername);
				break;
			case Transition.TextColor:
				StartTextColorTween(color, instant: false);
				break;
			}
		}
	}

	private void StartColorTween(Color targetColor, bool instant)
	{
		if (!(m_TargetGraphic == null))
		{
			if (instant || m_Duration == 0f || !Application.isPlaying)
			{
				m_TargetGraphic.canvasRenderer.SetColor(targetColor);
			}
			else
			{
				m_TargetGraphic.CrossFadeColor(targetColor, m_Duration, ignoreTimeScale: true, useAlpha: true);
			}
		}
	}

	private void DoSpriteSwap(Sprite newSprite)
	{
		Image image = m_TargetGraphic as Image;
		if (!(image == null))
		{
			image.overrideSprite = newSprite;
		}
	}

	private void TriggerAnimation(string triggername)
	{
		if (!(targetGameObject == null) && !(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
		{
			animator.ResetTrigger(m_PressedTrigger);
			animator.SetTrigger(triggername);
		}
	}

	private void StartTextColorTween(Color targetColor, bool instant)
	{
		if (!(m_TargetGraphic == null) && m_TargetGraphic is Text)
		{
			if (instant || m_Duration == 0f || !Application.isPlaying)
			{
				(m_TargetGraphic as Text).color = targetColor;
				return;
			}
			ColorTween colorTween = default(ColorTween);
			colorTween.duration = m_Duration;
			colorTween.startColor = (m_TargetGraphic as Text).color;
			colorTween.targetColor = targetColor;
			ColorTween colorTween2 = colorTween;
			colorTween2.AddOnChangedCallback(SetTextColor);
			colorTween2.ignoreTimeScale = true;
			m_ColorTweenRunner.StartTween(colorTween2);
		}
	}

	private void SetTextColor(Color targetColor)
	{
		if (!(m_TargetGraphic == null) && m_TargetGraphic is Text)
		{
			(m_TargetGraphic as Text).color = targetColor;
		}
	}
}
