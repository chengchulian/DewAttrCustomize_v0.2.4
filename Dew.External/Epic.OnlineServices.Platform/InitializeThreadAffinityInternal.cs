using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct InitializeThreadAffinityInternal : IGettable<InitializeThreadAffinity>, ISettable<InitializeThreadAffinity>, IDisposable
{
	private int m_ApiVersion;

	private ulong m_NetworkWork;

	private ulong m_StorageIo;

	private ulong m_WebSocketIo;

	private ulong m_P2PIo;

	private ulong m_HttpRequestIo;

	private ulong m_RTCIo;

	public ulong NetworkWork
	{
		get
		{
			return m_NetworkWork;
		}
		set
		{
			m_NetworkWork = value;
		}
	}

	public ulong StorageIo
	{
		get
		{
			return m_StorageIo;
		}
		set
		{
			m_StorageIo = value;
		}
	}

	public ulong WebSocketIo
	{
		get
		{
			return m_WebSocketIo;
		}
		set
		{
			m_WebSocketIo = value;
		}
	}

	public ulong P2PIo
	{
		get
		{
			return m_P2PIo;
		}
		set
		{
			m_P2PIo = value;
		}
	}

	public ulong HttpRequestIo
	{
		get
		{
			return m_HttpRequestIo;
		}
		set
		{
			m_HttpRequestIo = value;
		}
	}

	public ulong RTCIo
	{
		get
		{
			return m_RTCIo;
		}
		set
		{
			m_RTCIo = value;
		}
	}

	public void Set(ref InitializeThreadAffinity other)
	{
		m_ApiVersion = 2;
		NetworkWork = other.NetworkWork;
		StorageIo = other.StorageIo;
		WebSocketIo = other.WebSocketIo;
		P2PIo = other.P2PIo;
		HttpRequestIo = other.HttpRequestIo;
		RTCIo = other.RTCIo;
	}

	public void Set(ref InitializeThreadAffinity? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			NetworkWork = other.Value.NetworkWork;
			StorageIo = other.Value.StorageIo;
			WebSocketIo = other.Value.WebSocketIo;
			P2PIo = other.Value.P2PIo;
			HttpRequestIo = other.Value.HttpRequestIo;
			RTCIo = other.Value.RTCIo;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out InitializeThreadAffinity output)
	{
		output = default(InitializeThreadAffinity);
		output.Set(ref this);
	}
}
