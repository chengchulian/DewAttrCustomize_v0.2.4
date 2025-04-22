using UnityEngine;

namespace DuloGames.UI;

public class UIScrollRectExpander : MonoBehaviour
{
	[SerializeField]
	private float m_ExpandWidth;

	[SerializeField]
	private RectTransform m_Target;

	private bool m_Expanded;

	protected void OnEnable()
	{
		if (base.gameObject.activeSelf)
		{
			Collapse();
		}
	}

	protected void OnDisable()
	{
		if (!base.gameObject.activeSelf)
		{
			Expand();
		}
	}

	private void Expand()
	{
		if (!m_Expanded && !(m_Target == null))
		{
			m_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Target.rect.width + m_ExpandWidth);
			m_Expanded = true;
		}
	}

	private void Collapse()
	{
		if (m_Expanded && !(m_Target == null))
		{
			m_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Target.rect.width - m_ExpandWidth);
			m_Expanded = false;
		}
	}
}
