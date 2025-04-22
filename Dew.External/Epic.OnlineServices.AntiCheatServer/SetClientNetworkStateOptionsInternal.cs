using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetClientNetworkStateOptionsInternal : ISettable<SetClientNetworkStateOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ClientHandle;

	private int m_IsNetworkActive;

	public IntPtr ClientHandle
	{
		set
		{
			m_ClientHandle = value;
		}
	}

	public bool IsNetworkActive
	{
		set
		{
			Helper.Set(value, ref m_IsNetworkActive);
		}
	}

	public void Set(ref SetClientNetworkStateOptions other)
	{
		m_ApiVersion = 1;
		ClientHandle = other.ClientHandle;
		IsNetworkActive = other.IsNetworkActive;
	}

	public void Set(ref SetClientNetworkStateOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ClientHandle = other.Value.ClientHandle;
			IsNetworkActive = other.Value.IsNetworkActive;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientHandle);
	}
}
