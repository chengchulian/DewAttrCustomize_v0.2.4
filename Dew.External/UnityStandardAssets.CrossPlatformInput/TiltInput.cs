using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput;

public class TiltInput : MonoBehaviour
{
	public enum AxisOptions
	{
		ForwardAxis,
		SidewaysAxis
	}

	[Serializable]
	public class AxisMapping
	{
		public enum MappingType
		{
			NamedAxis,
			MousePositionX,
			MousePositionY,
			MousePositionZ
		}

		public MappingType type;

		public string axisName;
	}

	public AxisMapping mapping;

	public AxisOptions tiltAroundAxis;

	public float fullTiltAngle = 25f;

	public float centreAngleOffset;

	private CrossPlatformInputManager.VirtualAxis m_SteerAxis;

	private void OnEnable()
	{
		if (mapping.type == AxisMapping.MappingType.NamedAxis)
		{
			m_SteerAxis = new CrossPlatformInputManager.VirtualAxis(mapping.axisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_SteerAxis);
		}
	}

	private void Update()
	{
		float angle = 0f;
		if (Input.acceleration != Vector3.zero)
		{
			switch (tiltAroundAxis)
			{
			case AxisOptions.ForwardAxis:
				angle = Mathf.Atan2(Input.acceleration.x, 0f - Input.acceleration.y) * 57.29578f + centreAngleOffset;
				break;
			case AxisOptions.SidewaysAxis:
				angle = Mathf.Atan2(Input.acceleration.z, 0f - Input.acceleration.y) * 57.29578f + centreAngleOffset;
				break;
			}
		}
		float axisValue = Mathf.InverseLerp(0f - fullTiltAngle, fullTiltAngle, angle) * 2f - 1f;
		switch (mapping.type)
		{
		case AxisMapping.MappingType.NamedAxis:
			m_SteerAxis.Update(axisValue);
			break;
		case AxisMapping.MappingType.MousePositionX:
			CrossPlatformInputManager.SetVirtualMousePositionX(axisValue * (float)Screen.width);
			break;
		case AxisMapping.MappingType.MousePositionY:
			CrossPlatformInputManager.SetVirtualMousePositionY(axisValue * (float)Screen.width);
			break;
		case AxisMapping.MappingType.MousePositionZ:
			CrossPlatformInputManager.SetVirtualMousePositionZ(axisValue * (float)Screen.width);
			break;
		}
	}

	private void OnDisable()
	{
		m_SteerAxis.Remove();
	}
}
