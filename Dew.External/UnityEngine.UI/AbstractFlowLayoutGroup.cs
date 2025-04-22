using System.Collections.Generic;

namespace UnityEngine.UI;

[ExecuteAlways]
public abstract class AbstractFlowLayoutGroup : LayoutGroup
{
	[SerializeField]
	protected float m_Spacing;

	[SerializeField]
	protected float m_LineSpacing;

	[SerializeField]
	protected bool m_ChildControlWidth = true;

	[SerializeField]
	protected bool m_ChildControlHeight = true;

	[SerializeField]
	protected bool m_ChildScaleWidth;

	[SerializeField]
	protected bool m_ChildScaleHeight;

	[SerializeField]
	protected bool m_ReverseArrangement;

	public float spacing
	{
		get
		{
			return m_Spacing;
		}
		set
		{
			SetProperty(ref m_Spacing, value);
		}
	}

	public float lineSpacing
	{
		get
		{
			return m_LineSpacing;
		}
		set
		{
			SetProperty(ref m_LineSpacing, value);
		}
	}

	public virtual Vector2 Spacing => new Vector2(spacing, lineSpacing);

	public bool childControlWidth
	{
		get
		{
			return m_ChildControlWidth;
		}
		set
		{
			SetProperty(ref m_ChildControlWidth, value);
		}
	}

	public bool childControlHeight
	{
		get
		{
			return m_ChildControlHeight;
		}
		set
		{
			SetProperty(ref m_ChildControlHeight, value);
		}
	}

	public bool childScaleWidth
	{
		get
		{
			return m_ChildScaleWidth;
		}
		set
		{
			SetProperty(ref m_ChildScaleWidth, value);
		}
	}

	public bool childScaleHeight
	{
		get
		{
			return m_ChildScaleHeight;
		}
		set
		{
			SetProperty(ref m_ChildScaleHeight, value);
		}
	}

	public bool reverseArrangement
	{
		get
		{
			return m_ReverseArrangement;
		}
		set
		{
			SetProperty(ref m_ReverseArrangement, value);
		}
	}

	public float GetWidth => base.rectTransform.rect.width - (float)base.padding.horizontal;

	public float GetHeight => base.rectTransform.rect.height;

	protected override void Awake()
	{
		base.Awake();
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
	}

	protected void CalcAlongAxis(int axis, bool isVertical)
	{
		_ = base.rectTransform.rect.size[axis];
		if (axis != 0)
		{
			_ = base.padding.vertical;
		}
		else
		{
			_ = base.padding.horizontal;
		}
		float combinedPadding = ((axis == 0) ? base.padding.horizontal : base.padding.vertical);
		bool controlSize = ((axis == 0) ? m_ChildControlWidth : m_ChildControlHeight);
		bool useScale = ((axis == 0) ? m_ChildScaleWidth : m_ChildScaleHeight);
		float totalMin = combinedPadding;
		float totalPreferred = combinedPadding;
		float totalFlexible = 0f;
		bool alongOtherAxis = isVertical ^ (axis == 1);
		_ = base.rectChildren.Count;
		int otherAxis = ((axis == 0) ? 1 : 0);
		float otherInnerSize = base.rectTransform.rect.size[otherAxis] - (float)((otherAxis == 0) ? base.padding.horizontal : base.padding.vertical);
		bool useScaleOtherAxis = ((otherAxis == 0) ? m_ChildScaleWidth : m_ChildScaleHeight);
		int rowIndex = 0;
		float currentRowLength = 0f;
		float rowOffset = 0f;
		float rowSize = 0f;
		int num = (m_ReverseArrangement ? (base.rectChildren.Count - 1) : 0);
		int endIndex = ((!m_ReverseArrangement) ? base.rectChildren.Count : 0);
		int increment = ((!m_ReverseArrangement) ? 1 : (-1));
		for (int i = num; m_ReverseArrangement ? (i >= endIndex) : (i < endIndex); i += increment)
		{
			RectTransform child = base.rectChildren[i];
			GetChildSizes(child, axis, controlSize, childForceExpand: false, out var min, out var preferred, out var flexible);
			GetChildSizes(child, otherAxis, controlSize, childForceExpand: false, out var _, out var _, out var _);
			if (useScale)
			{
				float scaleFactor = child.localScale[axis];
				min *= scaleFactor;
				preferred *= scaleFactor;
				flexible *= scaleFactor;
			}
			if (alongOtherAxis)
			{
				totalMin = Mathf.Max(min + combinedPadding, totalMin);
				totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
				totalFlexible = Mathf.Max(flexible, totalFlexible);
				currentRowLength += child.sizeDelta[otherAxis] * (useScaleOtherAxis ? child.localScale[otherAxis] : 1f);
				if (currentRowLength > otherInnerSize)
				{
					rowIndex++;
					rowOffset += rowSize;
					rowSize = child.sizeDelta[axis] * (useScale ? child.localScale[axis] : 1f);
					currentRowLength = child.sizeDelta[otherAxis] * (useScaleOtherAxis ? child.localScale[otherAxis] : 1f);
				}
				else
				{
					rowSize = Mathf.Max(child.sizeDelta[axis] * (useScale ? child.localScale[axis] : 1f), rowSize);
				}
				currentRowLength += spacing;
			}
			else
			{
				totalMin += min + spacing;
				totalPreferred += preferred + spacing;
				totalFlexible += flexible;
			}
		}
		if (!alongOtherAxis && base.rectChildren.Count > 0)
		{
			totalMin -= spacing;
			totalPreferred -= spacing;
		}
		totalPreferred = Mathf.Max(totalMin, totalPreferred);
		if (alongOtherAxis)
		{
			rowOffset += rowSize;
			totalMin = totalPreferred;
			totalPreferred = rowOffset + lineSpacing * (float)rowIndex + combinedPadding;
		}
		if (!alongOtherAxis)
		{
			SetLayoutInputForAxis(totalFlexible, totalFlexible, totalFlexible, axis);
		}
		else
		{
			SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
		}
	}

	protected void SetChildrenAlongAxis(int axis, bool isVertical)
	{
		float size = base.rectTransform.rect.size[axis];
		float innerSize = size - (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical);
		bool controlSize = ((axis == 0) ? m_ChildControlWidth : m_ChildControlHeight);
		bool useScale = ((axis == 0) ? m_ChildScaleWidth : m_ChildScaleHeight);
		float alignmentOnAxis = GetAlignmentOnAxis(axis);
		bool num = isVertical ^ (axis == 1);
		int startIndex = (m_ReverseArrangement ? (base.rectChildren.Count - 1) : 0);
		int endIndex = ((!m_ReverseArrangement) ? base.rectChildren.Count : 0);
		int increment = ((!m_ReverseArrangement) ? 1 : (-1));
		int otherAxis = ((axis == 0) ? 1 : 0);
		bool useScaleOtherAxis = ((otherAxis == 0) ? m_ChildScaleWidth : m_ChildScaleHeight);
		float otherInnerSize = base.rectTransform.rect.size[otherAxis] - (float)((otherAxis == 0) ? base.padding.horizontal : base.padding.vertical);
		if (num)
		{
			float currentRowLength = 0f;
			int rowIndex = 0;
			float rowSize = 0f;
			float rowOffset = 0f;
			for (int i = startIndex; m_ReverseArrangement ? (i >= endIndex) : (i < endIndex); i += increment)
			{
				RectTransform child = base.rectChildren[i];
				GetChildSizes(child, axis, controlSize, childForceExpand: false, out var min, out var preferred, out var flexible);
				GetChildSizes(child, otherAxis, controlSize, childForceExpand: false, out var _, out var _, out var _);
				float scaleFactor = (useScale ? child.localScale[axis] : 1f);
				float requiredSpace = Mathf.Clamp(innerSize, min, (flexible > 0f) ? size : preferred);
				float startOffset = size - GetTotalPreferredSize(axis);
				startOffset = CalcOtherAxisOffset(startOffset);
				startOffset += (float)((axis == 0) ? base.padding.left : base.padding.top);
				currentRowLength += child.sizeDelta[otherAxis] * (useScaleOtherAxis ? child.localScale[otherAxis] : 1f);
				if (currentRowLength > otherInnerSize)
				{
					rowIndex++;
					rowOffset += rowSize;
					rowSize = child.sizeDelta[axis] * (useScale ? child.localScale[axis] : 1f);
					currentRowLength = child.sizeDelta[otherAxis] * (useScaleOtherAxis ? child.localScale[otherAxis] : 1f);
				}
				else
				{
					rowSize = Mathf.Max(child.sizeDelta[axis] * (useScaleOtherAxis ? child.localScale[axis] : 1f), rowSize);
				}
				currentRowLength += spacing;
				float rowPos = rowOffset + lineSpacing * (float)rowIndex;
				if (controlSize)
				{
					SetChildAlongAxisWithScale(child, axis, startOffset + rowPos, requiredSpace, scaleFactor);
					continue;
				}
				float offsetInCell = (requiredSpace - child.sizeDelta[axis]) * alignmentOnAxis;
				SetChildAlongAxisWithScale(child, axis, startOffset + offsetInCell + rowPos, scaleFactor);
			}
			return;
		}
		float pos = ((axis == 0) ? base.padding.left : base.padding.top);
		float itemFlexibleMultiplier = 0f;
		GetTotalPreferredSize(axis);
		float minMaxLerp = 0f;
		if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
		{
			minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));
		}
		List<List<RectTransform>> rows = DivideIntoRows();
		int rowIndex2 = 0;
		float currentRowLength2 = 0f;
		for (int j = startIndex; m_ReverseArrangement ? (j >= endIndex) : (j < endIndex); j += increment)
		{
			RectTransform child2 = base.rectChildren[j];
			GetChildSizes(child2, axis, controlSize, childForceExpand: false, out var min2, out var preferred2, out var flexible2);
			min2 = preferred2;
			currentRowLength2 += child2.sizeDelta[axis] * (useScale ? child2.localScale[axis] : 1f);
			if (currentRowLength2 > innerSize)
			{
				rowIndex2++;
				currentRowLength2 = child2.sizeDelta[axis] * (useScale ? child2.localScale[axis] : 1f);
				pos = ((axis == 0) ? base.padding.left : base.padding.top);
			}
			currentRowLength2 += spacing;
			float scaleFactor2 = (useScale ? child2.localScale[axis] : 1f);
			float childSize = Mathf.Lerp(min2, preferred2, minMaxLerp);
			childSize += flexible2 * itemFlexibleMultiplier;
			float posOffset = CalcRowOffset(rowIndex2);
			if (controlSize)
			{
				SetChildAlongAxisWithScale(child2, axis, pos + posOffset, childSize, scaleFactor2);
			}
			else
			{
				SetChildAlongAxisWithScale(child2, axis, pos + posOffset, scaleFactor2);
			}
			pos += childSize * scaleFactor2 + spacing;
		}
		float CalcOtherAxisOffset(float delta)
		{
			if (axis == 0)
			{
				if (base.childAlignment == TextAnchor.UpperLeft || base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.LowerLeft)
				{
					return 0f;
				}
				if (base.childAlignment == TextAnchor.UpperCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.LowerCenter)
				{
					return delta / 2f;
				}
				return delta;
			}
			if (base.childAlignment == TextAnchor.UpperLeft || base.childAlignment == TextAnchor.UpperCenter || base.childAlignment == TextAnchor.UpperRight)
			{
				return 0f;
			}
			if (base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.MiddleRight)
			{
				return delta / 2f;
			}
			return delta;
		}
		float CalcRowOffset(int index)
		{
			float rowSize2 = CalcRowSize(index);
			float delta2 = innerSize - rowSize2;
			if (axis == 0)
			{
				if (base.childAlignment == TextAnchor.UpperLeft || base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.LowerLeft)
				{
					return 0f;
				}
				if (base.childAlignment == TextAnchor.UpperCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.LowerCenter)
				{
					return delta2 / 2f;
				}
				return delta2;
			}
			if (base.childAlignment == TextAnchor.UpperLeft || base.childAlignment == TextAnchor.UpperCenter || base.childAlignment == TextAnchor.UpperRight)
			{
				return 0f;
			}
			if (base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.MiddleRight)
			{
				return delta2 / 2f;
			}
			return delta2;
		}
		float CalcRowSize(int index)
		{
			List<RectTransform> row = rows[index];
			float _size = 0f;
			foreach (RectTransform child4 in row)
			{
				_size += child4.sizeDelta[axis] * (useScale ? child4.localScale[axis] : 1f);
				_size += spacing;
			}
			if (row.Count > 0)
			{
				_size -= spacing;
			}
			return _size;
		}
		List<List<RectTransform>> DivideIntoRows()
		{
			int _rowIndex = 0;
			float _currentRowLength = 0f;
			List<List<RectTransform>> _rows = new List<List<RectTransform>>
			{
				new List<RectTransform>()
			};
			for (int k = startIndex; m_ReverseArrangement ? (k >= endIndex) : (k < endIndex); k += increment)
			{
				RectTransform child3 = base.rectChildren[k];
				GetChildSizes(child3, axis, controlSize, childForceExpand: false, out var min3, out var preferred3, out var _);
				min3 = preferred3;
				_currentRowLength += child3.sizeDelta[axis] * (useScale ? child3.localScale[axis] : 1f);
				if (_currentRowLength > innerSize)
				{
					_rowIndex++;
					_rows.Add(new List<RectTransform>());
					_currentRowLength = child3.sizeDelta[axis] * (useScale ? child3.localScale[axis] : 1f);
				}
				_rows[_rowIndex].Add(child3);
				_currentRowLength += spacing;
			}
			return _rows;
		}
	}

	private void GetChildSizes(RectTransform child, int axis, bool controlSize, bool childForceExpand, out float min, out float preferred, out float flexible)
	{
		if (!controlSize)
		{
			min = child.sizeDelta[axis];
			preferred = min;
			flexible = 0f;
		}
		else
		{
			min = LayoutUtility.GetMinSize(child, axis);
			preferred = LayoutUtility.GetPreferredSize(child, axis);
			flexible = LayoutUtility.GetFlexibleSize(child, axis);
		}
	}
}
