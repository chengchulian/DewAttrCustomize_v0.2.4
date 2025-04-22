using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ActiveSessionInfoInternal : IGettable<ActiveSessionInfo>, ISettable<ActiveSessionInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionName;

	private IntPtr m_LocalUserId;

	private OnlineSessionState m_State;

	private IntPtr m_SessionDetails;

	public Utf8String SessionName
	{
		get
		{
			Helper.Get(m_SessionName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SessionName);
		}
	}

	public ProductUserId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public OnlineSessionState State
	{
		get
		{
			return m_State;
		}
		set
		{
			m_State = value;
		}
	}

	public SessionDetailsInfo? SessionDetails
	{
		get
		{
			Helper.Get<SessionDetailsInfoInternal, SessionDetailsInfo>(m_SessionDetails, out SessionDetailsInfo? value);
			return value;
		}
		set
		{
			Helper.Set<SessionDetailsInfo, SessionDetailsInfoInternal>(ref value, ref m_SessionDetails);
		}
	}

	public void Set(ref ActiveSessionInfo other)
	{
		m_ApiVersion = 1;
		SessionName = other.SessionName;
		LocalUserId = other.LocalUserId;
		State = other.State;
		SessionDetails = other.SessionDetails;
	}

	public void Set(ref ActiveSessionInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SessionName = other.Value.SessionName;
			LocalUserId = other.Value.LocalUserId;
			State = other.Value.State;
			SessionDetails = other.Value.SessionDetails;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionName);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_SessionDetails);
	}

	public void Get(out ActiveSessionInfo output)
	{
		output = default(ActiveSessionInfo);
		output.Set(ref this);
	}
}
