using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	public enum AxisOption
	{
		Both,
		OnlyHorizontal,
		OnlyVertical
	}

	public int MovementRange = 100;

	public AxisOption axesToUse;

	public string horizontalAxisName = "Horizontal";

	public string verticalAxisName = "Vertical";

	private Vector3 m_StartPos;

	private bool m_UseX;

	private bool m_UseY;

	private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

	private void OnEnable()
	{
		CreateVirtualAxes();
	}

	private void Start()
	{
		m_StartPos = base.transform.position;
	}

	private void UpdateVirtualAxes(Vector3 value)
	{
		Vector3 delta = m_StartPos - value;
		delta.y = 0f - delta.y;
		delta /= (float)MovementRange;
		if (m_UseX)
		{
			m_HorizontalVirtualAxis.Update(0f - delta.x);
		}
		if (m_UseY)
		{
			m_VerticalVirtualAxis.Update(delta.y);
		}
	}

	private void CreateVirtualAxes()
	{
		m_UseX = axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal;
		m_UseY = axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical;
		if (m_UseX)
		{
			m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
		}
		if (m_UseY)
		{
			m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		Vector3 newPos = Vector3.zero;
		if (m_UseX)
		{
			int delta = (int)(data.position.x - m_StartPos.x);
			delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
			newPos.x = delta;
		}
		if (m_UseY)
		{
			int delta2 = (int)(data.position.y - m_StartPos.y);
			delta2 = Mathf.Clamp(delta2, -MovementRange, MovementRange);
			newPos.y = delta2;
		}
		base.transform.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
		UpdateVirtualAxes(base.transform.position);
	}

	public void OnPointerUp(PointerEventData data)
	{
		base.transform.position = m_StartPos;
		UpdateVirtualAxes(m_StartPos);
	}

	public void OnPointerDown(PointerEventData data)
	{
	}

	private void OnDisable()
	{
		if (m_UseX)
		{
			m_HorizontalVirtualAxis.Remove();
		}
		if (m_UseY)
		{
			m_VerticalVirtualAxis.Remove();
		}
	}
}
