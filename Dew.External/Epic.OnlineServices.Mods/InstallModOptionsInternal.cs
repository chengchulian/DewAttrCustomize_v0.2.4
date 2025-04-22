using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct InstallModOptionsInternal : ISettable<InstallModOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_Mod;

	private int m_RemoveAfterExit;

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

	public bool RemoveAfterExit
	{
		set
		{
			Helper.Set(value, ref m_RemoveAfterExit);
		}
	}

	public void Set(ref InstallModOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		Mod = other.Mod;
		RemoveAfterExit = other.RemoveAfterExit;
	}

	public void Set(ref InstallModOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			Mod = other.Value.Mod;
			RemoveAfterExit = other.Value.RemoveAfterExit;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Mod);
	}
}
