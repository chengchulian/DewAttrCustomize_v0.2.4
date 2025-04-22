using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[DisallowMultipleComponent]
[AddComponentMenu("UI/Tab", 58)]
public class UITab : Toggle
{
	public enum TextTransition
	{
		None,
		ColorTint
	}

	[SerializeField]
	private GameObject m_TargetContent;

	[SerializeField]
	private Image m_ImageTarget;

	[SerializeField]
	private Transition m_ImageTransition;

	[SerializeField]
	private ColorBlockExtended m_ImageColors = ColorBlockExtended.defaultColorBlock;

	[SerializeField]
	private SpriteStateExtended m_ImageSpriteState;

	[SerializeField]
	private AnimationTriggersExtended m_ImageAnimationTriggers = new AnimationTriggersExtended();

	[SerializeField]
	private Text m_TextTarget;

	[SerializeField]
	private TextTransition m_TextTransition;

	[SerializeField]
	private ColorBlockExtended m_TextColors = ColorBlockExtended.defaultColorBlock;

	private SelectionState m_CurrentState;

	[NonSerialized]
	private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

	public GameObject targetContent
	{
		get
		{
			return m_TargetContent;
		}
		set
		{
			m_TargetContent = value;
		}
	}

	public Image imageTarget
	{
		get
		{
			return m_ImageTarget;
		}
		set
		{
			m_ImageTarget = value;
		}
	}

	public Transition imageTransition
	{
		get
		{
			return m_ImageTransition;
		}
		set
		{
			m_ImageTransition = value;
		}
	}

	public ColorBlockExtended imageColors
	{
		get
		{
			return m_ImageColors;
		}
		set
		{
			m_ImageColors = value;
		}
	}

	public SpriteStateExtended imageSpriteState
	{
		get
		{
			return m_ImageSpriteState;
		}
		set
		{
			m_ImageSpriteState = value;
		}
	}

	public AnimationTriggersExtended imageAnimationTriggers
	{
		get
		{
			return m_ImageAnimationTriggers;
		}
		set
		{
			m_ImageAnimationTriggers = value;
		}
	}

	public Text textTarget
	{
		get
		{
			return m_TextTarget;
		}
		set
		{
			m_TextTarget = value;
		}
	}

	public TextTransition textTransition
	{
		get
		{
			return m_TextTransition;
		}
		set
		{
			m_TextTransition = value;
		}
	}

	public ColorBlockExtended textColors
	{
		get
		{
			return m_TextColors;
		}
		set
		{
			m_TextColors = value;
		}
	}

	protected UITab()
	{
		if (m_ColorTweenRunner == null)
		{
			m_ColorTweenRunner = new TweenRunner<ColorTween>();
		}
		m_ColorTweenRunner.Init(this);
	}

	protected override void Awake()
	{
		base.Awake();
		if (base.group == null)
		{
			ToggleGroup grp = UIUtility.FindInParents<ToggleGroup>(base.gameObject);
			if (grp != null)
			{
				base.group = grp;
			}
			else
			{
				base.group = base.transform.parent.gameObject.AddComponent<ToggleGroup>();
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		onValueChanged.AddListener(OnToggleStateChanged);
		InternalEvaluateAndTransitionState(instant: true);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		onValueChanged.RemoveListener(OnToggleStateChanged);
	}

	protected void OnToggleStateChanged(bool state)
	{
		if (IsActive() && Application.isPlaying)
		{
			InternalEvaluateAndTransitionState(instant: false);
		}
	}

	public void EvaluateAndToggleContent()
	{
		if (m_TargetContent != null)
		{
			m_TargetContent.SetActive(base.isOn);
		}
	}

	private void InternalEvaluateAndTransitionState(bool instant)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		EvaluateAndToggleContent();
		if (graphic != null && graphic.transform.childCount > 0)
		{
			float targetAlpha = ((!base.isOn) ? 0f : 1f);
			foreach (Transform item in graphic.transform)
			{
				Graphic g = item.GetComponent<Graphic>();
				if (g != null && !g.canvasRenderer.GetAlpha().Equals(targetAlpha))
				{
					if (instant)
					{
						g.canvasRenderer.SetAlpha(targetAlpha);
					}
					else
					{
						g.CrossFadeAlpha(targetAlpha, 0.1f, ignoreTimeScale: true);
					}
				}
			}
		}
		DoStateTransition(m_CurrentState, instant);
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		m_CurrentState = state;
		Color newImageColor = m_ImageColors.normalColor;
		Color newTextColor = m_TextColors.normalColor;
		Sprite newSprite = null;
		string imageTrigger = m_ImageAnimationTriggers.normalTrigger;
		switch (state)
		{
		case SelectionState.Normal:
			newImageColor = ((!base.isOn) ? m_ImageColors.normalColor : m_ImageColors.activeColor);
			newTextColor = ((!base.isOn) ? m_TextColors.normalColor : m_TextColors.activeColor);
			newSprite = ((!base.isOn) ? null : m_ImageSpriteState.activeSprite);
			imageTrigger = ((!base.isOn) ? m_ImageAnimationTriggers.normalTrigger : m_ImageAnimationTriggers.activeTrigger);
			break;
		case SelectionState.Highlighted:
			newImageColor = ((!base.isOn) ? m_ImageColors.highlightedColor : m_ImageColors.activeHighlightedColor);
			newTextColor = ((!base.isOn) ? m_TextColors.highlightedColor : m_TextColors.activeHighlightedColor);
			newSprite = ((!base.isOn) ? m_ImageSpriteState.highlightedSprite : m_ImageSpriteState.activeHighlightedSprite);
			imageTrigger = ((!base.isOn) ? m_ImageAnimationTriggers.highlightedTrigger : m_ImageAnimationTriggers.activeHighlightedTrigger);
			break;
		case SelectionState.Pressed:
			newImageColor = ((!base.isOn) ? m_ImageColors.pressedColor : m_ImageColors.activePressedColor);
			newTextColor = ((!base.isOn) ? m_TextColors.pressedColor : m_TextColors.activePressedColor);
			newSprite = ((!base.isOn) ? m_ImageSpriteState.pressedSprite : m_ImageSpriteState.activePressedSprite);
			imageTrigger = ((!base.isOn) ? m_ImageAnimationTriggers.pressedTrigger : m_ImageAnimationTriggers.activePressedTrigger);
			break;
		case SelectionState.Disabled:
			newImageColor = m_ImageColors.disabledColor;
			newTextColor = m_TextColors.disabledColor;
			newSprite = m_ImageSpriteState.disabledSprite;
			imageTrigger = m_ImageAnimationTriggers.disabledTrigger;
			break;
		}
		if (base.gameObject.activeInHierarchy)
		{
			switch (m_ImageTransition)
			{
			case Transition.ColorTint:
				StartColorTween(m_ImageTarget, newImageColor * m_ImageColors.colorMultiplier, instant ? 0f : m_ImageColors.fadeDuration);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(m_ImageTarget, newSprite);
				break;
			case Transition.Animation:
				TriggerAnimation(m_ImageTarget.gameObject, imageTrigger);
				break;
			}
			if (m_TextTransition == TextTransition.ColorTint)
			{
				StartColorTweenText(newTextColor * m_TextColors.colorMultiplier, instant ? 0f : m_TextColors.fadeDuration);
			}
		}
	}

	private void StartColorTween(Graphic target, Color targetColor, float duration)
	{
		if (!(target == null))
		{
			if (!Application.isPlaying || duration == 0f)
			{
				target.canvasRenderer.SetColor(targetColor);
			}
			else
			{
				target.CrossFadeColor(targetColor, duration, ignoreTimeScale: true, useAlpha: true);
			}
		}
	}

	private void StartColorTweenText(Color targetColor, float duration)
	{
		if (!(m_TextTarget == null))
		{
			if (!Application.isPlaying || duration == 0f)
			{
				m_TextTarget.color = targetColor;
				return;
			}
			ColorTween colorTween = default(ColorTween);
			colorTween.duration = duration;
			colorTween.startColor = m_TextTarget.color;
			colorTween.targetColor = targetColor;
			ColorTween colorTween2 = colorTween;
			colorTween2.AddOnChangedCallback(SetTextColor);
			colorTween2.ignoreTimeScale = true;
			m_ColorTweenRunner.StartTween(colorTween2);
		}
	}

	private void SetTextColor(Color color)
	{
		if (!(m_TextTarget == null))
		{
			m_TextTarget.color = color;
		}
	}

	private void DoSpriteSwap(Image target, Sprite newSprite)
	{
		if (!(target == null) && !target.overrideSprite.Equals(newSprite))
		{
			target.overrideSprite = newSprite;
		}
	}

	private void TriggerAnimation(GameObject target, string triggername)
	{
		if (!(target == null))
		{
			Animator animator = target.GetComponent<Animator>();
			if (!(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
			{
				animator.ResetTrigger(m_ImageAnimationTriggers.normalTrigger);
				animator.ResetTrigger(m_ImageAnimationTriggers.pressedTrigger);
				animator.ResetTrigger(m_ImageAnimationTriggers.highlightedTrigger);
				animator.ResetTrigger(m_ImageAnimationTriggers.activeTrigger);
				animator.ResetTrigger(m_ImageAnimationTriggers.activeHighlightedTrigger);
				animator.ResetTrigger(m_ImageAnimationTriggers.activePressedTrigger);
				animator.ResetTrigger(m_ImageAnimationTriggers.disabledTrigger);
				animator.SetTrigger(triggername);
			}
		}
	}

	public void Activate()
	{
		if (!base.isOn)
		{
			base.isOn = true;
		}
	}
}
