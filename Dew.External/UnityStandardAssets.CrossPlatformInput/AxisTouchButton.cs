using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput;

public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public string axisName = "Horizontal";

	public float axisValue = 1f;

	public float responseSpeed = 3f;

	public float returnToCentreSpeed = 3f;

	private AxisTouchButton m_PairedWith;

	private CrossPlatformInputManager.VirtualAxis m_Axis;

	private void OnEnable()
	{
		if (!CrossPlatformInputManager.AxisExists(axisName))
		{
			m_Axis = new CrossPlatformInputManager.VirtualAxis(axisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
		}
		else
		{
			m_Axis = CrossPlatformInputManager.VirtualAxisReference(axisName);
		}
		FindPairedButton();
	}

	private void FindPairedButton()
	{
		if (!(Object.FindObjectsOfType(typeof(AxisTouchButton)) is AxisTouchButton[] otherAxisButtons))
		{
			return;
		}
		for (int i = 0; i < otherAxisButtons.Length; i++)
		{
			if (otherAxisButtons[i].axisName == axisName && otherAxisButtons[i] != this)
			{
				m_PairedWith = otherAxisButtons[i];
			}
		}
	}

	private void OnDisable()
	{
		m_Axis.Remove();
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (m_PairedWith == null)
		{
			FindPairedButton();
		}
		m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, axisValue, responseSpeed * Time.deltaTime));
	}

	public void OnPointerUp(PointerEventData data)
	{
		m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0f, responseSpeed * Time.deltaTime));
	}
}
