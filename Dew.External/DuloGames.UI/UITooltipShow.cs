using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Tooltip Show", 58)]
[DisallowMultipleComponent]
public class UITooltipShow : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum Position
	{
		Floating,
		Anchored
	}

	public enum WidthMode
	{
		Default,
		Preferred
	}

	[SerializeField]
	private Position m_Position;

	[SerializeField]
	private WidthMode m_WidthMode;

	[SerializeField]
	private bool m_OverrideOffset;

	[SerializeField]
	private Vector2 m_Offset = Vector2.zero;

	[SerializeField]
	[Tooltip("How long of a delay to expect before showing the tooltip.")]
	[Range(0f, 10f)]
	private float m_Delay = 1f;

	[SerializeField]
	private UITooltipLineContent[] m_ContentLines = new UITooltipLineContent[0];

	private bool m_IsTooltipShown;

	public UITooltipLineContent[] contentLines
	{
		get
		{
			return m_ContentLines;
		}
		set
		{
			m_ContentLines = value;
		}
	}

	public virtual void OnTooltip(bool show)
	{
		if (!base.enabled || !IsActive())
		{
			return;
		}
		if (show)
		{
			UITooltip.InstantiateIfNecessary(base.gameObject);
			for (int i = 0; i < m_ContentLines.Length; i++)
			{
				UITooltipLineContent line = m_ContentLines[i];
				if (line.IsSpacer)
				{
					UITooltip.AddSpacer();
				}
				else if (line.LineStyle != UITooltipLines.LineStyle.Custom)
				{
					UITooltip.AddLine(line.Content, line.LineStyle);
				}
				else
				{
					UITooltip.AddLine(line.Content, line.CustomLineStyle);
				}
			}
			if (m_WidthMode == WidthMode.Preferred)
			{
				UITooltip.SetHorizontalFitMode(ContentSizeFitter.FitMode.PreferredSize);
			}
			if (m_Position == Position.Anchored)
			{
				UITooltip.AnchorToRect(base.transform as RectTransform);
			}
			if (m_OverrideOffset)
			{
				UITooltip.OverrideOffset(m_Offset);
				UITooltip.OverrideAnchoredOffset(m_Offset);
			}
			UITooltip.Show();
		}
		else
		{
			UITooltip.Hide();
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (base.enabled && IsActive())
		{
			UITooltip.InstantiateIfNecessary(base.gameObject);
			if (m_Delay > 0f)
			{
				StartCoroutine("DelayedShow");
			}
			else
			{
				InternalShowTooltip();
			}
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		InternalHideTooltip();
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		InternalHideTooltip();
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
	}

	protected void InternalShowTooltip()
	{
		if (!m_IsTooltipShown)
		{
			m_IsTooltipShown = true;
			OnTooltip(show: true);
		}
	}

	protected void InternalHideTooltip()
	{
		StopCoroutine("DelayedShow");
		if (m_IsTooltipShown)
		{
			m_IsTooltipShown = false;
			OnTooltip(show: false);
		}
	}

	protected IEnumerator DelayedShow()
	{
		yield return new WaitForSeconds(m_Delay);
		InternalShowTooltip();
	}
}
