using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RegisterPeerOptionsInternal : ISettable<RegisterPeerOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PeerHandle;

	private AntiCheatCommonClientType m_ClientType;

	private AntiCheatCommonClientPlatform m_ClientPlatform;

	private uint m_AuthenticationTimeout;

	private IntPtr m_AccountId_DEPRECATED;

	private IntPtr m_IpAddress;

	private IntPtr m_PeerProductUserId;

	public IntPtr PeerHandle
	{
		set
		{
			m_PeerHandle = value;
		}
	}

	public AntiCheatCommonClientType ClientType
	{
		set
		{
			m_ClientType = value;
		}
	}

	public AntiCheatCommonClientPlatform ClientPlatform
	{
		set
		{
			m_ClientPlatform = value;
		}
	}

	public uint AuthenticationTimeout
	{
		set
		{
			m_AuthenticationTimeout = value;
		}
	}

	public Utf8String AccountId_DEPRECATED
	{
		set
		{
			Helper.Set(value, ref m_AccountId_DEPRECATED);
		}
	}

	public Utf8String IpAddress
	{
		set
		{
			Helper.Set(value, ref m_IpAddress);
		}
	}

	public ProductUserId PeerProductUserId
	{
		set
		{
			Helper.Set(value, ref m_PeerProductUserId);
		}
	}

	public void Set(ref RegisterPeerOptions other)
	{
		m_ApiVersion = 3;
		PeerHandle = other.PeerHandle;
		ClientType = other.ClientType;
		ClientPlatform = other.ClientPlatform;
		AuthenticationTimeout = other.AuthenticationTimeout;
		AccountId_DEPRECATED = other.AccountId_DEPRECATED;
		IpAddress = other.IpAddress;
		PeerProductUserId = other.PeerProductUserId;
	}

	public void Set(ref RegisterPeerOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			PeerHandle = other.Value.PeerHandle;
			ClientType = other.Value.ClientType;
			ClientPlatform = other.Value.ClientPlatform;
			AuthenticationTimeout = other.Value.AuthenticationTimeout;
			AccountId_DEPRECATED = other.Value.AccountId_DEPRECATED;
			IpAddress = other.Value.IpAddress;
			PeerProductUserId = other.Value.PeerProductUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PeerHandle);
		Helper.Dispose(ref m_AccountId_DEPRECATED);
		Helper.Dispose(ref m_IpAddress);
		Helper.Dispose(ref m_PeerProductUserId);
	}
}
