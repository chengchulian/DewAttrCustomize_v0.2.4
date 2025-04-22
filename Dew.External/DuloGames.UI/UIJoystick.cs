using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI;

[AddComponentMenu("UI/Joystick", 36)]
[RequireComponent(typeof(RectTransform))]
public class UIJoystick : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum AxisOption
	{
		Both,
		OnlyHorizontal,
		OnlyVertical
	}

	[SerializeField]
	[Tooltip("The child graphic that will be moved around.")]
	private RectTransform m_Handle;

	[SerializeField]
	[Tooltip("The handling area that the handle is allowed to be moved in.")]
	private RectTransform m_HandlingArea;

	[SerializeField]
	[Tooltip("The child graphic that will be shown when the joystick is active.")]
	private Image m_ActiveGraphic;

	[SerializeField]
	private Vector2 m_Axis;

	[SerializeField]
	[Tooltip("How fast the joystick will go back to the center")]
	private float m_Spring = 25f;

	[SerializeField]
	[Tooltip("How close to the center that the axis will be output as 0")]
	private float m_DeadZone = 0.1f;

	[Tooltip("Customize the output that is sent in OnValueChange")]
	public AnimationCurve outputCurve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));

	private bool m_IsDragging;

	public RectTransform Handle
	{
		get
		{
			return m_Handle;
		}
		set
		{
			m_Handle = value;
			UpdateHandle();
		}
	}

	public RectTransform HandlingArea
	{
		get
		{
			return m_HandlingArea;
		}
		set
		{
			m_HandlingArea = value;
		}
	}

	public Image ActiveGraphic
	{
		get
		{
			return m_ActiveGraphic;
		}
		set
		{
			m_ActiveGraphic = value;
		}
	}

	public float Spring
	{
		get
		{
			return m_Spring;
		}
		set
		{
			m_Spring = value;
		}
	}

	public float DeadZone
	{
		get
		{
			return m_DeadZone;
		}
		set
		{
			m_DeadZone = value;
		}
	}

	public Vector2 JoystickAxis
	{
		get
		{
			Vector2 outputPoint = ((m_Axis.magnitude > m_DeadZone) ? m_Axis : Vector2.zero);
			float magnitude = outputPoint.magnitude;
			return outputPoint * outputCurve.Evaluate(magnitude);
		}
		set
		{
			SetAxis(value);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		CreateVirtualAxes();
		if (m_HandlingArea == null)
		{
			m_HandlingArea = base.transform as RectTransform;
		}
		if (m_ActiveGraphic != null)
		{
			m_ActiveGraphic.canvasRenderer.SetAlpha(0f);
		}
	}

	protected void CreateVirtualAxes()
	{
	}

	protected void LateUpdate()
	{
		if (base.isActiveAndEnabled && !m_IsDragging && m_Axis != Vector2.zero)
		{
			Vector2 newAxis = m_Axis - m_Axis * Time.unscaledDeltaTime * m_Spring;
			if (newAxis.sqrMagnitude <= 0.0001f)
			{
				newAxis = Vector2.zero;
			}
			SetAxis(newAxis);
		}
	}

	public void SetAxis(Vector2 axis)
	{
		m_Axis = Vector2.ClampMagnitude(axis, 1f);
		Vector2 outputPoint = ((m_Axis.magnitude > m_DeadZone) ? m_Axis : Vector2.zero);
		float magnitude = outputPoint.magnitude;
		outputPoint *= outputCurve.Evaluate(magnitude);
		UpdateHandle();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (IsActive() && !(m_HandlingArea == null))
		{
			Vector2 newAxis = m_HandlingArea.InverseTransformPoint(eventData.position);
			newAxis.x /= m_HandlingArea.sizeDelta.x * 0.5f;
			newAxis.y /= m_HandlingArea.sizeDelta.y * 0.5f;
			SetAxis(newAxis);
			m_IsDragging = true;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_IsDragging = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!(m_HandlingArea == null))
		{
			Vector2 axis = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandlingArea, eventData.position, eventData.pressEventCamera, out axis);
			axis -= m_HandlingArea.rect.center;
			axis.x /= m_HandlingArea.sizeDelta.x * 0.5f;
			axis.y /= m_HandlingArea.sizeDelta.y * 0.5f;
			SetAxis(axis);
		}
	}

	private void UpdateHandle()
	{
		if ((bool)m_Handle && (bool)m_HandlingArea)
		{
			m_Handle.anchoredPosition = new Vector2(m_Axis.x * m_HandlingArea.sizeDelta.x * 0.5f, m_Axis.y * m_HandlingArea.sizeDelta.y * 0.5f);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (m_ActiveGraphic != null)
		{
			m_ActiveGraphic.CrossFadeAlpha(1f, 0.2f, ignoreTimeScale: false);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (m_ActiveGraphic != null)
		{
			m_ActiveGraphic.CrossFadeAlpha(0f, 0.2f, ignoreTimeScale: false);
		}
	}
}
