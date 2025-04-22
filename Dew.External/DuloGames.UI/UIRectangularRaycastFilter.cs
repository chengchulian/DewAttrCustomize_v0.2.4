using UnityEngine;

namespace DuloGames.UI;

[AddComponentMenu("UI/Raycast Filters/Rectangular Raycast Filter")]
[RequireComponent(typeof(RectTransform))]
public class UIRectangularRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
	[SerializeField]
	private Vector2 m_Offset = Vector2.zero;

	[SerializeField]
	private RectOffset m_Borders = new RectOffset();

	[Range(0f, 1f)]
	[SerializeField]
	private float m_ScaleX = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float m_ScaleY = 1f;

	public Vector2 offset
	{
		get
		{
			return m_Offset;
		}
		set
		{
			m_Offset = value;
		}
	}

	public RectOffset borders
	{
		get
		{
			return m_Borders;
		}
		set
		{
			m_Borders = value;
		}
	}

	public float scaleX
	{
		get
		{
			return m_ScaleX;
		}
		set
		{
			m_ScaleX = value;
		}
	}

	public float scaleY
	{
		get
		{
			return m_ScaleY;
		}
		set
		{
			m_ScaleY = value;
		}
	}

	public Rect scaledRect
	{
		get
		{
			RectTransform rt = (RectTransform)base.transform;
			return new Rect(offset.x + (float)borders.left + (rt.rect.x + (rt.rect.width - rt.rect.width * m_ScaleX) / 2f), offset.y + (float)borders.bottom + (rt.rect.y + (rt.rect.height - rt.rect.height * m_ScaleY) / 2f), rt.rect.width * m_ScaleX - (float)borders.left - (float)borders.right, rt.rect.height * m_ScaleY - (float)borders.top - (float)borders.bottom);
		}
	}

	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		if (!base.enabled)
		{
			return true;
		}
		RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, screenPoint, eventCamera, out var localPositionPivotRelative);
		return scaledRect.Contains(localPositionPivotRelative);
	}
}
