using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct JoinSessionOptionsInternal : ISettable<JoinSessionOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionName;

	private IntPtr m_SessionHandle;

	private IntPtr m_LocalUserId;

	private int m_PresenceEnabled;

	public Utf8String SessionName
	{
		set
		{
			Helper.Set(value, ref m_SessionName);
		}
	}

	public SessionDetails SessionHandle
	{
		set
		{
			Helper.Set(value, ref m_SessionHandle);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public bool PresenceEnabled
	{
		set
		{
			Helper.Set(value, ref m_PresenceEnabled);
		}
	}

	public void Set(ref JoinSessionOptions other)
	{
		m_ApiVersion = 2;
		SessionName = other.SessionName;
		SessionHandle = other.SessionHandle;
		LocalUserId = other.LocalUserId;
		PresenceEnabled = other.PresenceEnabled;
	}

	public void Set(ref JoinSessionOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			SessionName = other.Value.SessionName;
			SessionHandle = other.Value.SessionHandle;
			LocalUserId = other.Value.LocalUserId;
			PresenceEnabled = other.Value.PresenceEnabled;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionName);
		Helper.Dispose(ref m_SessionHandle);
		Helper.Dispose(ref m_LocalUserId);
	}
}
