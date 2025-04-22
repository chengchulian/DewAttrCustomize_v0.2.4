using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct BeginSessionOptionsInternal : ISettable<BeginSessionOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private AntiCheatClientMode m_Mode;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public AntiCheatClientMode Mode
	{
		set
		{
			m_Mode = value;
		}
	}

	public void Set(ref BeginSessionOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		Mode = other.Mode;
	}

	public void Set(ref BeginSessionOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			Mode = other.Value.Mode;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
