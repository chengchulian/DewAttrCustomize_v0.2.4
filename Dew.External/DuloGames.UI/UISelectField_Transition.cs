using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
[AddComponentMenu("UI/Select Field - Transition", 58)]
[RequireComponent(typeof(UISelectField))]
public class UISelectField_Transition : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Graphic that will have the selected transtion applied.")]
	private Graphic m_TargetGraphic;

	[SerializeField]
	[Tooltip("GameObject that will have the selected transtion applied.")]
	private GameObject m_TargetGameObject;

	[SerializeField]
	private Selectable.Transition m_Transition;

	[SerializeField]
	private ColorBlockExtended m_Colors = ColorBlockExtended.defaultColorBlock;

	[SerializeField]
	private SpriteStateExtended m_SpriteState;

	[SerializeField]
	private AnimationTriggersExtended m_AnimationTriggers = new AnimationTriggersExtended();

	private UISelectField m_Select;

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

	public Selectable.Transition transition
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

	public ColorBlockExtended colors
	{
		get
		{
			return m_Colors;
		}
		set
		{
			m_Colors = value;
		}
	}

	public SpriteStateExtended spriteState
	{
		get
		{
			return m_SpriteState;
		}
		set
		{
			m_SpriteState = value;
		}
	}

	public AnimationTriggersExtended animationTriggers
	{
		get
		{
			return m_AnimationTriggers;
		}
		set
		{
			m_AnimationTriggers = value;
		}
	}

	protected void Awake()
	{
		m_Select = base.gameObject.GetComponent<UISelectField>();
	}

	protected void OnEnable()
	{
		if (m_Select != null)
		{
			m_Select.onTransition.AddListener(OnTransition);
		}
		OnTransition(UISelectField.VisualState.Normal, instant: true);
	}

	protected void OnDisable()
	{
		if (m_Select != null)
		{
			m_Select.onTransition.RemoveListener(OnTransition);
		}
		InstantClearState();
	}

	protected void InstantClearState()
	{
		switch (m_Transition)
		{
		case Selectable.Transition.ColorTint:
			StartColorTween(Color.white, instant: true);
			break;
		case Selectable.Transition.SpriteSwap:
			DoSpriteSwap(null);
			break;
		}
	}

	public void OnTransition(UISelectField.VisualState state, bool instant)
	{
		if ((!(targetGraphic == null) || !(targetGameObject == null)) && base.gameObject.activeInHierarchy && m_Transition != 0)
		{
			Color color = colors.normalColor;
			Sprite newSprite = null;
			string triggername = animationTriggers.normalTrigger;
			switch (state)
			{
			case UISelectField.VisualState.Normal:
				color = colors.normalColor;
				newSprite = null;
				triggername = animationTriggers.normalTrigger;
				break;
			case UISelectField.VisualState.Highlighted:
				color = colors.highlightedColor;
				newSprite = spriteState.highlightedSprite;
				triggername = animationTriggers.highlightedTrigger;
				break;
			case UISelectField.VisualState.Pressed:
				color = colors.pressedColor;
				newSprite = spriteState.pressedSprite;
				triggername = animationTriggers.pressedTrigger;
				break;
			case UISelectField.VisualState.Active:
				color = colors.activeColor;
				newSprite = spriteState.activeSprite;
				triggername = animationTriggers.activeTrigger;
				break;
			case UISelectField.VisualState.ActiveHighlighted:
				color = colors.activeHighlightedColor;
				newSprite = spriteState.activeHighlightedSprite;
				triggername = animationTriggers.activeHighlightedTrigger;
				break;
			case UISelectField.VisualState.ActivePressed:
				color = colors.activePressedColor;
				newSprite = spriteState.activePressedSprite;
				triggername = animationTriggers.activePressedTrigger;
				break;
			case UISelectField.VisualState.Disabled:
				color = colors.disabledColor;
				newSprite = spriteState.disabledSprite;
				triggername = animationTriggers.disabledTrigger;
				break;
			}
			switch (m_Transition)
			{
			case Selectable.Transition.ColorTint:
				StartColorTween(color * colors.colorMultiplier, instant || colors.fadeDuration == 0f);
				break;
			case Selectable.Transition.SpriteSwap:
				DoSpriteSwap(newSprite);
				break;
			case Selectable.Transition.Animation:
				TriggerAnimation(triggername);
				break;
			}
		}
	}

	private void StartColorTween(Color color, bool instant)
	{
		if (!(targetGraphic == null))
		{
			if (instant)
			{
				targetGraphic.canvasRenderer.SetColor(color);
			}
			else
			{
				targetGraphic.CrossFadeColor(color, colors.fadeDuration, ignoreTimeScale: true, useAlpha: true);
			}
		}
	}

	private void DoSpriteSwap(Sprite newSprite)
	{
		if (!(targetGraphic == null))
		{
			Image image = targetGraphic as Image;
			if (image != null)
			{
				image.overrideSprite = newSprite;
			}
		}
	}

	private void TriggerAnimation(string trigger)
	{
		Animator animator = GetComponent<Animator>();
		if (!(animator == null) && animator.enabled && animator.isActiveAndEnabled && !(animator.runtimeAnimatorController == null) && animator.hasBoundPlayables && !string.IsNullOrEmpty(trigger))
		{
			animator.ResetTrigger(animationTriggers.normalTrigger);
			animator.ResetTrigger(animationTriggers.pressedTrigger);
			animator.ResetTrigger(animationTriggers.highlightedTrigger);
			animator.ResetTrigger(animationTriggers.activeTrigger);
			animator.ResetTrigger(animationTriggers.activeHighlightedTrigger);
			animator.ResetTrigger(animationTriggers.activePressedTrigger);
			animator.ResetTrigger(animationTriggers.disabledTrigger);
			animator.SetTrigger(trigger);
		}
	}
}
