using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[AddComponentMenu("UI/Toggle Active Transition")]
public class UIToggleActiveTransition : MonoBehaviour, IEventSystemHandler
{
	public enum VisualState
	{
		Normal,
		Active
	}

	public enum Transition
	{
		None,
		ColorTint,
		SpriteSwap,
		Animation,
		TextColor,
		CanvasGroup
	}

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private Color m_NormalColor = new Color(1f, 1f, 1f, 0f);

	[SerializeField]
	private Color m_ActiveColor = Color.white;

	[SerializeField]
	private float m_Duration = 0.1f;

	[SerializeField]
	[Range(1f, 6f)]
	private float m_ColorMultiplier = 1f;

	[SerializeField]
	private Sprite m_ActiveSprite;

	[SerializeField]
	private string m_NormalTrigger = "Normal";

	[SerializeField]
	private string m_ActiveBool = "Active";

	[SerializeField]
	[Range(0f, 1f)]
	private float m_NormalAlpha;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_ActiveAlpha = 1f;

	[SerializeField]
	[Tooltip("Graphic that will have the selected transtion applied.")]
	private Graphic m_TargetGraphic;

	[SerializeField]
	[Tooltip("GameObject that will have the selected transtion applied.")]
	private GameObject m_TargetGameObject;

	[SerializeField]
	[Tooltip("CanvasGroup that will have the selected transtion applied.")]
	private CanvasGroup m_TargetCanvasGroup;

	[SerializeField]
	private Toggle m_TargetToggle;

	private bool m_Active;

	[NonSerialized]
	private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

	[NonSerialized]
	private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

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

	public CanvasGroup targetCanvasGroup
	{
		get
		{
			return m_TargetCanvasGroup;
		}
		set
		{
			m_TargetCanvasGroup = value;
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

	protected UIToggleActiveTransition()
	{
		if (m_ColorTweenRunner == null)
		{
			m_ColorTweenRunner = new TweenRunner<ColorTween>();
		}
		if (m_FloatTweenRunner == null)
		{
			m_FloatTweenRunner = new TweenRunner<FloatTween>();
		}
		m_ColorTweenRunner.Init(this);
		m_FloatTweenRunner.Init(this);
	}

	protected void Awake()
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

	private void InternalEvaluateAndTransitionToNormalState(bool instant)
	{
		DoStateTransition(m_Active ? VisualState.Active : VisualState.Normal, instant);
	}

	protected void OnToggleValueChange(bool value)
	{
		if (m_TargetToggle == null)
		{
			return;
		}
		m_Active = m_TargetToggle.isOn;
		if (m_Transition == Transition.Animation)
		{
			if (targetGameObject == null || animator == null || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null || string.IsNullOrEmpty(m_ActiveBool))
			{
				return;
			}
			animator.SetBool(m_ActiveBool, m_Active);
		}
		DoStateTransition(m_Active ? VisualState.Active : VisualState.Normal, instant: false);
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
			SetTextColor(m_NormalColor);
			break;
		case Transition.CanvasGroup:
			SetCanvasGroupAlpha(1f);
			break;
		case Transition.Animation:
			break;
		}
	}

	protected virtual void DoStateTransition(VisualState state, bool instant)
	{
		if (base.gameObject.activeInHierarchy)
		{
			Color color = m_NormalColor;
			Sprite newSprite = null;
			string triggername = m_NormalTrigger;
			float alpha = m_NormalAlpha;
			switch (state)
			{
			case VisualState.Normal:
				color = m_NormalColor;
				newSprite = null;
				triggername = m_NormalTrigger;
				alpha = m_NormalAlpha;
				break;
			case VisualState.Active:
				color = m_ActiveColor;
				newSprite = m_ActiveSprite;
				triggername = m_NormalTrigger;
				alpha = m_ActiveAlpha;
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
			case Transition.CanvasGroup:
				StartCanvasGroupTween(alpha, instant);
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
		if (!(targetGameObject == null) && !(animator == null) && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
		{
			animator.ResetTrigger(m_NormalTrigger);
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

	private void StartCanvasGroupTween(float targetAlpha, bool instant)
	{
		if (!(m_TargetCanvasGroup == null))
		{
			if (instant || m_Duration == 0f || !Application.isPlaying)
			{
				SetCanvasGroupAlpha(targetAlpha);
				return;
			}
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = m_Duration;
			floatTween.startFloat = m_TargetCanvasGroup.alpha;
			floatTween.targetFloat = targetAlpha;
			FloatTween floatTween2 = floatTween;
			floatTween2.AddOnChangedCallback(SetCanvasGroupAlpha);
			floatTween2.ignoreTimeScale = true;
			m_FloatTweenRunner.StartTween(floatTween2);
		}
	}

	private void SetCanvasGroupAlpha(float alpha)
	{
		if (!(m_TargetCanvasGroup == null))
		{
			m_TargetCanvasGroup.alpha = alpha;
		}
	}
}
