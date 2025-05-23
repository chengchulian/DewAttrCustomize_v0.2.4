using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ReportInputStateOptionsInternal : ISettable<ReportInputStateOptions>, IDisposable
{
	private int m_ApiVersion;

	private InputStateButtonFlags m_ButtonDownFlags;

	private int m_AcceptIsFaceButtonRight;

	private int m_MouseButtonDown;

	private uint m_MousePosX;

	private uint m_MousePosY;

	private uint m_GamepadIndex;

	private float m_LeftStickX;

	private float m_LeftStickY;

	private float m_RightStickX;

	private float m_RightStickY;

	private float m_LeftTrigger;

	private float m_RightTrigger;

	public InputStateButtonFlags ButtonDownFlags
	{
		set
		{
			m_ButtonDownFlags = value;
		}
	}

	public bool AcceptIsFaceButtonRight
	{
		set
		{
			Helper.Set(value, ref m_AcceptIsFaceButtonRight);
		}
	}

	public bool MouseButtonDown
	{
		set
		{
			Helper.Set(value, ref m_MouseButtonDown);
		}
	}

	public uint MousePosX
	{
		set
		{
			m_MousePosX = value;
		}
	}

	public uint MousePosY
	{
		set
		{
			m_MousePosY = value;
		}
	}

	public uint GamepadIndex
	{
		set
		{
			m_GamepadIndex = value;
		}
	}

	public float LeftStickX
	{
		set
		{
			m_LeftStickX = value;
		}
	}

	public float LeftStickY
	{
		set
		{
			m_LeftStickY = value;
		}
	}

	public float RightStickX
	{
		set
		{
			m_RightStickX = value;
		}
	}

	public float RightStickY
	{
		set
		{
			m_RightStickY = value;
		}
	}

	public float LeftTrigger
	{
		set
		{
			m_LeftTrigger = value;
		}
	}

	public float RightTrigger
	{
		set
		{
			m_RightTrigger = value;
		}
	}

	public void Set(ref ReportInputStateOptions other)
	{
		m_ApiVersion = 2;
		ButtonDownFlags = other.ButtonDownFlags;
		AcceptIsFaceButtonRight = other.AcceptIsFaceButtonRight;
		MouseButtonDown = other.MouseButtonDown;
		MousePosX = other.MousePosX;
		MousePosY = other.MousePosY;
		GamepadIndex = other.GamepadIndex;
		LeftStickX = other.LeftStickX;
		LeftStickY = other.LeftStickY;
		RightStickX = other.RightStickX;
		RightStickY = other.RightStickY;
		LeftTrigger = other.LeftTrigger;
		RightTrigger = other.RightTrigger;
	}

	public void Set(ref ReportInputStateOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			ButtonDownFlags = other.Value.ButtonDownFlags;
			AcceptIsFaceButtonRight = other.Value.AcceptIsFaceButtonRight;
			MouseButtonDown = other.Value.MouseButtonDown;
			MousePosX = other.Value.MousePosX;
			MousePosY = other.Value.MousePosY;
			GamepadIndex = other.Value.GamepadIndex;
			LeftStickX = other.Value.LeftStickX;
			LeftStickY = other.Value.LeftStickY;
			RightStickX = other.Value.RightStickX;
			RightStickY = other.Value.RightStickY;
			LeftTrigger = other.Value.LeftTrigger;
			RightTrigger = other.Value.RightTrigger;
		}
	}

	public void Dispose()
	{
	}
}
