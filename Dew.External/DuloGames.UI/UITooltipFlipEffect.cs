using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI;

public class UITooltipFlipEffect : MonoBehaviour
{
	[SerializeField]
	private UITooltip m_Tooltip;

	[SerializeField]
	private Graphic m_Graphic;

	private UIFlippable m_Flippable;

	private Vector2 m_OriginalPivot;

	private Vector2 m_OriginalAnchorMin;

	private Vector2 m_OriginalAnchorMax;

	private Vector2 m_OriginalPosition;

	protected void Awake()
	{
		if (!(m_Graphic == null) && !(m_Tooltip == null))
		{
			m_Flippable = m_Graphic.gameObject.GetComponent<UIFlippable>();
			if (m_Flippable == null)
			{
				m_Flippable = m_Graphic.gameObject.AddComponent<UIFlippable>();
			}
			m_OriginalPivot = m_Graphic.rectTransform.pivot;
			m_OriginalAnchorMin = m_Graphic.rectTransform.anchorMin;
			m_OriginalAnchorMax = m_Graphic.rectTransform.anchorMax;
			m_OriginalPosition = m_Graphic.rectTransform.anchoredPosition;
		}
	}

	protected void OnEnable()
	{
		if (!(m_Graphic == null) && !(m_Tooltip == null))
		{
			m_Tooltip.onAnchorEvent.AddListener(OnAnchor);
		}
	}

	protected void OnDisable()
	{
		if (!(m_Graphic == null) && !(m_Tooltip == null))
		{
			m_Tooltip.onAnchorEvent.RemoveListener(OnAnchor);
		}
	}

	public void OnAnchor(UITooltip.Anchor anchor)
	{
		if (!(m_Graphic == null) && !(m_Flippable == null))
		{
			RectTransform rt = m_Graphic.rectTransform;
			switch (anchor)
			{
			case UITooltip.Anchor.BottomLeft:
			case UITooltip.Anchor.TopLeft:
			case UITooltip.Anchor.Left:
			case UITooltip.Anchor.Bottom:
				m_Flippable.horizontal = false;
				rt.pivot = m_OriginalPivot;
				rt.anchorMin = m_OriginalAnchorMin;
				rt.anchorMax = m_OriginalAnchorMax;
				rt.anchoredPosition = m_OriginalPosition;
				break;
			case UITooltip.Anchor.BottomRight:
			case UITooltip.Anchor.TopRight:
			case UITooltip.Anchor.Right:
				m_Flippable.horizontal = true;
				rt.pivot = new Vector2((m_OriginalPivot.x == 0f) ? 1f : 0f, m_OriginalPivot.y);
				rt.anchorMin = new Vector2((m_OriginalAnchorMin.x == 0f) ? 1f : 0f, m_OriginalAnchorMin.y);
				rt.anchorMax = new Vector2((m_OriginalAnchorMax.x == 0f) ? 1f : 0f, m_OriginalAnchorMax.y);
				rt.anchoredPosition = new Vector2(m_OriginalPosition.x * -1f, m_OriginalPosition.y);
				break;
			case UITooltip.Anchor.Top:
				m_Flippable.vertical = true;
				rt.pivot = new Vector2(m_OriginalPivot.x, (m_OriginalPivot.y == 0f) ? 1f : 0f);
				rt.anchorMin = new Vector2(m_OriginalAnchorMin.x, (m_OriginalAnchorMin.y == 0f) ? 1f : 0f);
				rt.anchorMax = new Vector2(m_OriginalAnchorMax.x, (m_OriginalAnchorMax.y == 0f) ? 1f : 0f);
				rt.anchoredPosition = new Vector2(m_OriginalPosition.x, m_OriginalPosition.y * -1f);
				break;
			}
		}
	}
}
