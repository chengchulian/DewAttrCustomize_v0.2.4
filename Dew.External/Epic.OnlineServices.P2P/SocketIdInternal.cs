using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SocketIdInternal : IGettable<SocketId>, ISettable<SocketId>, IDisposable
{
	private int m_ApiVersion;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
	private byte[] m_SocketName;

	public string SocketName
	{
		get
		{
			Helper.Get(m_SocketName, out string value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SocketName, 33);
		}
	}

	public void Set(ref SocketId other)
	{
		m_ApiVersion = 1;
		SocketName = other.SocketName;
	}

	public void Set(ref SocketId? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SocketName = other.Value.SocketName;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out SocketId output)
	{
		output = default(SocketId);
		output.Set(ref this);
	}
}
