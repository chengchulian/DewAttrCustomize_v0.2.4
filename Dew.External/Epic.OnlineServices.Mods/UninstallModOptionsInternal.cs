using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UninstallModOptionsInternal : ISettable<UninstallModOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_Mod;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ModIdentifier? Mod
	{
		set
		{
			Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref m_Mod);
		}
	}

	public void Set(ref UninstallModOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		Mod = other.Mod;
	}

	public void Set(ref UninstallModOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			Mod = other.Value.Mod;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Mod);
	}
}
