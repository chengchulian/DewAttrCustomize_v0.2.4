using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetClientDetailsOptionsInternal : ISettable<SetClientDetailsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ClientHandle;

	private AntiCheatCommonClientFlags m_ClientFlags;

	private AntiCheatCommonClientInput m_ClientInputMethod;

	public IntPtr ClientHandle
	{
		set
		{
			m_ClientHandle = value;
		}
	}

	public AntiCheatCommonClientFlags ClientFlags
	{
		set
		{
			m_ClientFlags = value;
		}
	}

	public AntiCheatCommonClientInput ClientInputMethod
	{
		set
		{
			m_ClientInputMethod = value;
		}
	}

	public void Set(ref SetClientDetailsOptions other)
	{
		m_ApiVersion = 1;
		ClientHandle = other.ClientHandle;
		ClientFlags = other.ClientFlags;
		ClientInputMethod = other.ClientInputMethod;
	}

	public void Set(ref SetClientDetailsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ClientHandle = other.Value.ClientHandle;
			ClientFlags = other.Value.ClientFlags;
			ClientInputMethod = other.Value.ClientInputMethod;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientHandle);
	}
}
