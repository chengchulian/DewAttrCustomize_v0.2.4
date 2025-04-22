using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct BeginPlayerSessionOptionsInternal : ISettable<BeginPlayerSessionOptions>, IDisposable
{
	private int m_ApiVersion;

	private BeginPlayerSessionOptionsAccountIdInternal m_AccountId;

	private IntPtr m_DisplayName;

	private UserControllerType m_ControllerType;

	private IntPtr m_ServerIp;

	private IntPtr m_GameSessionId;

	public BeginPlayerSessionOptionsAccountId AccountId
	{
		set
		{
			Helper.Set(ref value, ref m_AccountId);
		}
	}

	public Utf8String DisplayName
	{
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public UserControllerType ControllerType
	{
		set
		{
			m_ControllerType = value;
		}
	}

	public Utf8String ServerIp
	{
		set
		{
			Helper.Set(value, ref m_ServerIp);
		}
	}

	public Utf8String GameSessionId
	{
		set
		{
			Helper.Set(value, ref m_GameSessionId);
		}
	}

	public void Set(ref BeginPlayerSessionOptions other)
	{
		m_ApiVersion = 1;
		AccountId = other.AccountId;
		DisplayName = other.DisplayName;
		ControllerType = other.ControllerType;
		ServerIp = other.ServerIp;
		GameSessionId = other.GameSessionId;
	}

	public void Set(ref BeginPlayerSessionOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AccountId = other.Value.AccountId;
			DisplayName = other.Value.DisplayName;
			ControllerType = other.Value.ControllerType;
			ServerIp = other.Value.ServerIp;
			GameSessionId = other.Value.GameSessionId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AccountId);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_ServerIp);
		Helper.Dispose(ref m_GameSessionId);
	}
}
