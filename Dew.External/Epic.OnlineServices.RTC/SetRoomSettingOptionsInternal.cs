using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetRoomSettingOptionsInternal : ISettable<SetRoomSettingOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private IntPtr m_SettingName;

	private IntPtr m_SettingValue;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String RoomName
	{
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public Utf8String SettingName
	{
		set
		{
			Helper.Set(value, ref m_SettingName);
		}
	}

	public Utf8String SettingValue
	{
		set
		{
			Helper.Set(value, ref m_SettingValue);
		}
	}

	public void Set(ref SetRoomSettingOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		SettingName = other.SettingName;
		SettingValue = other.SettingValue;
	}

	public void Set(ref SetRoomSettingOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			SettingName = other.Value.SettingName;
			SettingValue = other.Value.SettingValue;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_SettingName);
		Helper.Dispose(ref m_SettingValue);
	}
}
