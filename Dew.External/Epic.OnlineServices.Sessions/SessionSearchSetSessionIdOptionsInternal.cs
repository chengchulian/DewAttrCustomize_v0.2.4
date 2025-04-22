using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionSearchSetSessionIdOptionsInternal : ISettable<SessionSearchSetSessionIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionId;

	public Utf8String SessionId
	{
		set
		{
			Helper.Set(value, ref m_SessionId);
		}
	}

	public void Set(ref SessionSearchSetSessionIdOptions other)
	{
		m_ApiVersion = 1;
		SessionId = other.SessionId;
	}

	public void Set(ref SessionSearchSetSessionIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SessionId = other.Value.SessionId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionId);
	}
}
