using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI;

[AddComponentMenu("UI/Drag Object", 82)]
public class UIDragObject : UIBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public enum Rounding
	{
		Soft,
		Hard
	}

	[Serializable]
	public class BeginDragEvent : UnityEvent<BaseEventData>
	{
	}

	[Serializable]
	public class EndDragEvent : UnityEvent<BaseEventData>
	{
	}

	[Serializable]
	public class DragEvent : UnityEvent<BaseEventData>
	{
	}

	[SerializeField]
	private RectTransform m_Target;

	[SerializeField]
	private bool m_Horizontal = true;

	[SerializeField]
	private bool m_Vertical = true;

	[SerializeField]
	private bool m_Inertia = true;

	[SerializeField]
	private Rounding m_InertiaRounding = Rounding.Hard;

	[SerializeField]
	private float m_DampeningRate = 9f;

	[SerializeField]
	private bool m_ConstrainWithinCanvas;

	[SerializeField]
	private bool m_ConstrainDrag = true;

	[SerializeField]
	private bool m_ConstrainInertia = true;

	private Canvas m_Canvas;

	private RectTransform m_CanvasRectTransform;

	private Vector2 m_PointerStartPosition = Vector2.zero;

	private Vector2 m_TargetStartPosition = Vector2.zero;

	private Vector2 m_Velocity;

	private bool m_Dragging;

	private Vector2 m_LastPosition = Vector2.zero;

	public BeginDragEvent onBeginDrag = new BeginDragEvent();

	public EndDragEvent onEndDrag = new EndDragEvent();

	public DragEvent onDrag = new DragEvent();

	public RectTransform target
	{
		get
		{
			return m_Target;
		}
		set
		{
			m_Target = value;
		}
	}

	public bool horizontal
	{
		get
		{
			return m_Horizontal;
		}
		set
		{
			m_Horizontal = value;
		}
	}

	public bool vertical
	{
		get
		{
			return m_Vertical;
		}
		set
		{
			m_Vertical = value;
		}
	}

	public bool inertia
	{
		get
		{
			return m_Inertia;
		}
		set
		{
			m_Inertia = value;
		}
	}

	public float dampeningRate
	{
		get
		{
			return m_DampeningRate;
		}
		set
		{
			m_DampeningRate = value;
		}
	}

	public bool constrainWithinCanvas
	{
		get
		{
			return m_ConstrainWithinCanvas;
		}
		set
		{
			m_ConstrainWithinCanvas = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		m_Canvas = UIUtility.FindInParents<Canvas>((m_Target != null) ? m_Target.gameObject : base.gameObject);
		if (m_Canvas != null)
		{
			m_CanvasRectTransform = m_Canvas.transform as RectTransform;
		}
	}

	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		m_Canvas = UIUtility.FindInParents<Canvas>((m_Target != null) ? m_Target.gameObject : base.gameObject);
		if (m_Canvas != null)
		{
			m_CanvasRectTransform = m_Canvas.transform as RectTransform;
		}
	}

	public override bool IsActive()
	{
		if (base.IsActive())
		{
			return m_Target != null;
		}
		return false;
	}

	public void StopMovement()
	{
		m_Velocity = Vector2.zero;
	}

	public void OnBeginDrag(PointerEventData data)
	{
		if (IsActive())
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(m_CanvasRectTransform, data.position, data.pressEventCamera, out m_PointerStartPosition);
			m_TargetStartPosition = m_Target.anchoredPosition;
			m_Velocity = Vector2.zero;
			m_Dragging = true;
			if (onBeginDrag != null)
			{
				onBeginDrag.Invoke(data);
			}
		}
	}

	public void OnEndDrag(PointerEventData data)
	{
		m_Dragging = false;
		if (IsActive() && onEndDrag != null)
		{
			onEndDrag.Invoke(data);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (IsActive() && !(m_Canvas == null))
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(m_CanvasRectTransform, data.position, data.pressEventCamera, out var mousePos);
			if (m_ConstrainWithinCanvas && m_ConstrainDrag)
			{
				mousePos = ClampToCanvas(mousePos);
			}
			Vector2 newPosition = m_TargetStartPosition + (mousePos - m_PointerStartPosition);
			if (!m_Horizontal)
			{
				newPosition.x = m_Target.anchoredPosition.x;
			}
			if (!m_Vertical)
			{
				newPosition.y = m_Target.anchoredPosition.y;
			}
			m_Target.anchoredPosition = newPosition;
			if (onDrag != null)
			{
				onDrag.Invoke(data);
			}
		}
	}

	protected virtual void LateUpdate()
	{
		if (!m_Target)
		{
			return;
		}
		if (m_Dragging && m_Inertia)
		{
			Vector3 to = (m_Target.anchoredPosition - m_LastPosition) / Time.unscaledDeltaTime;
			m_Velocity = Vector3.Lerp(m_Velocity, to, Time.unscaledDeltaTime * 10f);
		}
		m_LastPosition = m_Target.anchoredPosition;
		if (m_Dragging || !(m_Velocity != Vector2.zero))
		{
			return;
		}
		Vector2 anchoredPosition = m_Target.anchoredPosition;
		Dampen(ref m_Velocity, m_DampeningRate, Time.unscaledDeltaTime);
		for (int i = 0; i < 2; i++)
		{
			if (m_Inertia)
			{
				anchoredPosition[i] += m_Velocity[i] * Time.unscaledDeltaTime;
			}
			else
			{
				m_Velocity[i] = 0f;
			}
		}
		if (!(m_Velocity != Vector2.zero))
		{
			return;
		}
		if (!m_Horizontal)
		{
			anchoredPosition.x = m_Target.anchoredPosition.x;
		}
		if (!m_Vertical)
		{
			anchoredPosition.y = m_Target.anchoredPosition.y;
		}
		if (m_ConstrainWithinCanvas && m_ConstrainInertia && m_CanvasRectTransform != null)
		{
			Vector3[] canvasCorners = new Vector3[4];
			m_CanvasRectTransform.GetWorldCorners(canvasCorners);
			Vector3[] targetCorners = new Vector3[4];
			m_Target.GetWorldCorners(targetCorners);
			if (targetCorners[0].x < canvasCorners[0].x || targetCorners[2].x > canvasCorners[2].x)
			{
				anchoredPosition.x = m_Target.anchoredPosition.x;
			}
			if (targetCorners[3].y < canvasCorners[3].y || targetCorners[1].y > canvasCorners[1].y)
			{
				anchoredPosition.y = m_Target.anchoredPosition.y;
			}
		}
		if (anchoredPosition != m_Target.anchoredPosition)
		{
			Rounding inertiaRounding = m_InertiaRounding;
			if (inertiaRounding != 0 && inertiaRounding == Rounding.Hard)
			{
				m_Target.anchoredPosition = new Vector2(Mathf.Round(anchoredPosition.x / 2f) * 2f, Mathf.Round(anchoredPosition.y / 2f) * 2f);
			}
			else
			{
				m_Target.anchoredPosition = new Vector2(Mathf.Round(anchoredPosition.x), Mathf.Round(anchoredPosition.y));
			}
		}
	}

	protected Vector3 Dampen(ref Vector2 velocity, float strength, float delta)
	{
		if (delta > 1f)
		{
			delta = 1f;
		}
		float dampeningFactor = 1f - strength * 0.001f;
		int ms = Mathf.RoundToInt(delta * 1000f);
		float totalDampening = Mathf.Pow(dampeningFactor, ms);
		Vector2 vector = velocity * ((totalDampening - 1f) / Mathf.Log(dampeningFactor));
		velocity *= totalDampening;
		return vector * 0.06f;
	}

	protected Vector2 ClampToScreen(Vector2 position)
	{
		if (m_Canvas != null && (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay || m_Canvas.renderMode == RenderMode.ScreenSpaceCamera))
		{
			float x = Mathf.Clamp(position.x, 0f, Screen.width);
			float clampedY = Mathf.Clamp(position.y, 0f, Screen.height);
			return new Vector2(x, clampedY);
		}
		return position;
	}

	protected Vector2 ClampToCanvas(Vector2 position)
	{
		if (m_CanvasRectTransform != null)
		{
			Vector3[] corners = new Vector3[4];
			m_CanvasRectTransform.GetLocalCorners(corners);
			float x = Mathf.Clamp(position.x, corners[0].x, corners[2].x);
			float clampedY = Mathf.Clamp(position.y, corners[3].y, corners[1].y);
			return new Vector2(x, clampedY);
		}
		return position;
	}
}
