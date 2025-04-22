using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

public class UISelectField_List : Selectable
{
	public enum State
	{
		Opened,
		Closed
	}

	[Serializable]
	public class AnimationFinishEvent : UnityEvent<State>
	{
	}

	public AnimationFinishEvent onAnimationFinish = new AnimationFinishEvent();

	public UnityEvent onDimensionsChange = new UnityEvent();

	private string m_AnimationOpenTrigger = string.Empty;

	private string m_AnimationCloseTrigger = string.Empty;

	private State m_State = State.Closed;

	protected override void Start()
	{
		base.Start();
		base.transition = Transition.None;
		Navigation nav = default(Navigation);
		nav.mode = Navigation.Mode.None;
		base.navigation = nav;
	}

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (onDimensionsChange != null)
		{
			onDimensionsChange.Invoke();
		}
	}

	public void SetTriggers(string openTrigger, string closeTrigger)
	{
		m_AnimationOpenTrigger = openTrigger;
		m_AnimationCloseTrigger = closeTrigger;
	}

	protected void Update()
	{
		if (!(base.animator != null) || string.IsNullOrEmpty(m_AnimationOpenTrigger) || string.IsNullOrEmpty(m_AnimationCloseTrigger))
		{
			return;
		}
		AnimatorStateInfo state = base.animator.GetCurrentAnimatorStateInfo(0);
		if (state.IsName(m_AnimationOpenTrigger) && m_State == State.Closed)
		{
			if (state.normalizedTime >= state.length)
			{
				m_State = State.Opened;
				if (onAnimationFinish != null)
				{
					onAnimationFinish.Invoke(m_State);
				}
			}
		}
		else if (state.IsName(m_AnimationCloseTrigger) && m_State == State.Opened && state.normalizedTime >= state.length)
		{
			m_State = State.Closed;
			if (onAnimationFinish != null)
			{
				onAnimationFinish.Invoke(m_State);
			}
		}
	}

	public new bool IsPressed()
	{
		return base.IsPressed();
	}

	public bool IsHighlighted(BaseEventData eventData)
	{
		return IsHighlighted();
	}
}
