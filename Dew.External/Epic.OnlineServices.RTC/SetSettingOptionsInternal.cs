using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetSettingOptionsInternal : ISettable<SetSettingOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SettingName;

	private IntPtr m_SettingValue;

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

	public void Set(ref SetSettingOptions other)
	{
		m_ApiVersion = 1;
		SettingName = other.SettingName;
		SettingValue = other.SettingValue;
	}

	public void Set(ref SetSettingOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SettingName = other.Value.SettingName;
			SettingValue = other.Value.SettingValue;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SettingName);
		Helper.Dispose(ref m_SettingValue);
	}
}
