using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ParseConnectStringOptionsInternal : ISettable<ParseConnectStringOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ConnectString;

	public Utf8String ConnectString
	{
		set
		{
			Helper.Set(value, ref m_ConnectString);
		}
	}

	public void Set(ref ParseConnectStringOptions other)
	{
		m_ApiVersion = 1;
		ConnectString = other.ConnectString;
	}

	public void Set(ref ParseConnectStringOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ConnectString = other.Value.ConnectString;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ConnectString);
	}
}
