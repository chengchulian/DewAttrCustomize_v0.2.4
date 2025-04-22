using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerUseWeaponOptionsInternal : ISettable<LogPlayerUseWeaponOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UseWeaponData;

	public LogPlayerUseWeaponData? UseWeaponData
	{
		set
		{
			Helper.Set<LogPlayerUseWeaponData, LogPlayerUseWeaponDataInternal>(ref value, ref m_UseWeaponData);
		}
	}

	public void Set(ref LogPlayerUseWeaponOptions other)
	{
		m_ApiVersion = 2;
		UseWeaponData = other.UseWeaponData;
	}

	public void Set(ref LogPlayerUseWeaponOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			UseWeaponData = other.Value.UseWeaponData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UseWeaponData);
	}
}
