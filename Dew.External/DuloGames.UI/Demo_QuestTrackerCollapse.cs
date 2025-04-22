using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[ExecuteInEditMode]
public class Demo_QuestTrackerCollapse : UIBehaviour
{
	[SerializeField]
	private GameObject m_Content;

	[SerializeField]
	private Toggle m_Toggle;

	[SerializeField]
	private UIFlippable m_ArrowFlippable;

	[SerializeField]
	private UIFlippable m_ArrowFlippable2;

	[SerializeField]
	private bool m_ArrowInvertFlip;

	[SerializeField]
	private Vector2 m_ActiveOffset = Vector2.zero;

	[SerializeField]
	private Vector2 m_InactiveOffset = Vector2.zero;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (m_Toggle != null)
		{
			m_Toggle.onValueChanged.AddListener(OnToggleStateChange);
		}
		if (m_Toggle != null)
		{
			OnToggleStateChange(m_Toggle.isOn);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (m_Toggle != null)
		{
			m_Toggle.onValueChanged.RemoveListener(OnToggleStateChange);
		}
		OnToggleStateChange(state: false);
	}

	public void OnToggleStateChange(bool state)
	{
		if (!IsActive())
		{
			return;
		}
		if (state)
		{
			if (m_Content != null)
			{
				m_Content.SetActive(value: true);
			}
			if (m_ArrowFlippable != null)
			{
				m_ArrowFlippable.vertical = !m_ArrowInvertFlip;
				(m_ArrowFlippable.transform as RectTransform).anchoredPosition = m_ActiveOffset;
			}
			if (m_ArrowFlippable2 != null)
			{
				m_ArrowFlippable2.vertical = !m_ArrowInvertFlip;
			}
		}
		else
		{
			if (m_Content != null)
			{
				m_Content.SetActive(value: false);
			}
			if (m_ArrowFlippable != null)
			{
				m_ArrowFlippable.vertical = (m_ArrowInvertFlip ? true : false);
				(m_ArrowFlippable.transform as RectTransform).anchoredPosition = m_InactiveOffset;
			}
			if (m_ArrowFlippable2 != null)
			{
				m_ArrowFlippable2.vertical = (m_ArrowInvertFlip ? true : false);
			}
		}
	}
}
