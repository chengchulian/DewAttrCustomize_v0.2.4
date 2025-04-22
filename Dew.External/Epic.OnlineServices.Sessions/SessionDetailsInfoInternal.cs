using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionDetailsInfoInternal : IGettable<SessionDetailsInfo>, ISettable<SessionDetailsInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionId;

	private IntPtr m_HostAddress;

	private uint m_NumOpenPublicConnections;

	private IntPtr m_Settings;

	private IntPtr m_OwnerUserId;

	private IntPtr m_OwnerServerClientId;

	public Utf8String SessionId
	{
		get
		{
			Helper.Get(m_SessionId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SessionId);
		}
	}

	public Utf8String HostAddress
	{
		get
		{
			Helper.Get(m_HostAddress, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_HostAddress);
		}
	}

	public uint NumOpenPublicConnections
	{
		get
		{
			return m_NumOpenPublicConnections;
		}
		set
		{
			m_NumOpenPublicConnections = value;
		}
	}

	public SessionDetailsSettings? Settings
	{
		get
		{
			Helper.Get<SessionDetailsSettingsInternal, SessionDetailsSettings>(m_Settings, out SessionDetailsSettings? value);
			return value;
		}
		set
		{
			Helper.Set<SessionDetailsSettings, SessionDetailsSettingsInternal>(ref value, ref m_Settings);
		}
	}

	public ProductUserId OwnerUserId
	{
		get
		{
			Helper.Get(m_OwnerUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OwnerUserId);
		}
	}

	public Utf8String OwnerServerClientId
	{
		get
		{
			Helper.Get(m_OwnerServerClientId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OwnerServerClientId);
		}
	}

	public void Set(ref SessionDetailsInfo other)
	{
		m_ApiVersion = 2;
		SessionId = other.SessionId;
		HostAddress = other.HostAddress;
		NumOpenPublicConnections = other.NumOpenPublicConnections;
		Settings = other.Settings;
		OwnerUserId = other.OwnerUserId;
		OwnerServerClientId = other.OwnerServerClientId;
	}

	public void Set(ref SessionDetailsInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			SessionId = other.Value.SessionId;
			HostAddress = other.Value.HostAddress;
			NumOpenPublicConnections = other.Value.NumOpenPublicConnections;
			Settings = other.Value.Settings;
			OwnerUserId = other.Value.OwnerUserId;
			OwnerServerClientId = other.Value.OwnerServerClientId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionId);
		Helper.Dispose(ref m_HostAddress);
		Helper.Dispose(ref m_Settings);
		Helper.Dispose(ref m_OwnerUserId);
		Helper.Dispose(ref m_OwnerServerClientId);
	}

	public void Get(out SessionDetailsInfo output)
	{
		output = default(SessionDetailsInfo);
		output.Set(ref this);
	}
}
