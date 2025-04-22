using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnMessageToClientCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnMessageToClientCallbackInfo>, ISettable<OnMessageToClientCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_ClientHandle;

	private IntPtr m_MessageData;

	private uint m_MessageDataSizeBytes;

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

	public ArraySegment<byte> MessageData
	{
		get
		{
			Helper.Get(m_MessageData, out var value, m_MessageDataSizeBytes);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_MessageData, out m_MessageDataSizeBytes);
		}
	}

	public void Set(ref OnMessageToClientCallbackInfo other)
	{
		ClientData = other.ClientData;
		ClientHandle = other.ClientHandle;
		MessageData = other.MessageData;
	}

	public void Set(ref OnMessageToClientCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			ClientHandle = other.Value.ClientHandle;
			MessageData = other.Value.MessageData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_ClientHandle);
		Helper.Dispose(ref m_MessageData);
	}

	public void Get(out OnMessageToClientCallbackInfo output)
	{
		output = default(OnMessageToClientCallbackInfo);
		output.Set(ref this);
	}
}
