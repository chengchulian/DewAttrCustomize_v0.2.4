using System;

namespace UnityEngine.UI;

[AddComponentMenu("Layout/Dew Content Size Fitter", 141)]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class DewContentSizeFitter : ContentSizeFitter
{
	[NonSerialized]
	private RectTransform m_Rect;

	[SerializeField]
	private float m_MaxWidth = -1f;

	[SerializeField]
	private float m_MaxHeight = -1f;

	private RectTransform rectTransform
	{
		get
		{
			if (m_Rect == null)
			{
				m_Rect = GetComponent<RectTransform>();
			}
			return m_Rect;
		}
	}

	public float maxWidth
	{
		get
		{
			return m_MaxWidth;
		}
		set
		{
			m_MaxWidth = value;
		}
	}

	public float maxHeight
	{
		get
		{
			return m_MaxHeight;
		}
		set
		{
			m_MaxHeight = value;
		}
	}

	public override void SetLayoutHorizontal()
	{
		base.SetLayoutHorizontal();
		if (maxWidth > 0f)
		{
			if (base.horizontalFit == FitMode.MinSize)
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Min(LayoutUtility.GetMinSize(m_Rect, 0), maxWidth));
			}
			else if (base.horizontalFit == FitMode.PreferredSize)
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Min(LayoutUtility.GetPreferredSize(m_Rect, 0), maxWidth));
			}
		}
	}

	public override void SetLayoutVertical()
	{
		base.SetLayoutVertical();
		if (maxHeight > 0f)
		{
			if (base.verticalFit == FitMode.MinSize)
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Min(LayoutUtility.GetMinSize(m_Rect, 1), maxHeight));
			}
			else if (base.verticalFit == FitMode.PreferredSize)
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Min(LayoutUtility.GetPreferredSize(m_Rect, 1), maxHeight));
			}
		}
	}
}
