using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnClientAuthStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientAuthStatusChangedCallbackInfo>, ISettable<OnClientAuthStatusChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_ClientHandle;

	private AntiCheatCommonClientAuthStatus m_ClientAuthStatus;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public IntPtr ClientHandle
	{
		get
		{
			return m_ClientHandle;
		}
		set
		{
			m_ClientHandle = value;
		}
	}

	public AntiCheatCommonClientAuthStatus ClientAuthStatus
	{
		get
		{
			return m_ClientAuthStatus;
		}
		set
		{
			m_ClientAuthStatus = value;
		}
	}

	public void Set(ref OnClientAuthStatusChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
		ClientHandle = other.ClientHandle;
		ClientAuthStatus = other.ClientAuthStatus;
	}

	public void Set(ref OnClientAuthStatusChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			ClientHandle = other.Value.ClientHandle;
			ClientAuthStatus = other.Value.ClientAuthStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_ClientHandle);
	}

	public void Get(out OnClientAuthStatusChangedCallbackInfo output)
	{
		output = default(OnClientAuthStatusChangedCallbackInfo);
		output.Set(ref this);
	}
}
