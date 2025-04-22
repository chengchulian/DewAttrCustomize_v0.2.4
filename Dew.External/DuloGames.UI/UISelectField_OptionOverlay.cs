using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

public class UISelectField_OptionOverlay : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum VisualState
	{
		Normal,
		Highlighted,
		Selected,
		Pressed
	}

	public enum Transition
	{
		None,
		ColorTint
	}

	[SerializeField]
	private Transition m_Transition;

	[SerializeField]
	private ColorBlock m_ColorBlock = ColorBlock.defaultColorBlock;

	[SerializeField]
	[Tooltip("Graphic that will have the selected transtion applied.")]
	private Graphic m_TargetGraphic;

	private bool m_Highlighted;

	private bool m_Selected;

	private bool m_Pressed;

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

	public ColorBlock colorBlock
	{
		get
		{
			return m_ColorBlock;
		}
		set
		{
			m_ColorBlock = value;
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

	protected void OnEnable()
	{
		InternalEvaluateAndTransitionToNormalState(instant: true);
	}

	protected void OnDisable()
	{
		InstantClearState();
	}

	protected void InstantClearState()
	{
		if (m_Transition == Transition.ColorTint)
		{
			StartColorTween(Color.white, instant: true);
		}
	}

	public void InternalEvaluateAndTransitionToNormalState(bool instant)
	{
		DoStateTransition(VisualState.Normal, instant);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		m_Highlighted = true;
		if (!m_Selected && !m_Pressed)
		{
			DoStateTransition(VisualState.Highlighted, instant: false);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_Highlighted = false;
		if (!m_Selected && !m_Pressed)
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
			if (m_Selected)
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
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			Color color = m_ColorBlock.normalColor;
			switch (state)
			{
			case VisualState.Normal:
				color = m_ColorBlock.normalColor;
				break;
			case VisualState.Highlighted:
				color = m_ColorBlock.highlightedColor;
				break;
			case VisualState.Pressed:
				color = m_ColorBlock.pressedColor;
				break;
			}
			if (m_Transition == Transition.ColorTint)
			{
				StartColorTween(color * m_ColorBlock.colorMultiplier, instant);
			}
		}
	}

	private void StartColorTween(Color targetColor, bool instant)
	{
		if (!(m_TargetGraphic == null))
		{
			if (instant || m_ColorBlock.fadeDuration == 0f || !Application.isPlaying)
			{
				m_TargetGraphic.canvasRenderer.SetColor(targetColor);
			}
			else
			{
				m_TargetGraphic.CrossFadeColor(targetColor, m_ColorBlock.fadeDuration, ignoreTimeScale: true, useAlpha: true);
			}
		}
	}
}
