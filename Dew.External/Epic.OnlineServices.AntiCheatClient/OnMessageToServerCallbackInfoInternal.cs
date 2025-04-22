using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnMessageToServerCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnMessageToServerCallbackInfo>, ISettable<OnMessageToServerCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

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

	public void Set(ref OnMessageToServerCallbackInfo other)
	{
		ClientData = other.ClientData;
		MessageData = other.MessageData;
	}

	public void Set(ref OnMessageToServerCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			MessageData = other.Value.MessageData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_MessageData);
	}

	public void Get(out OnMessageToServerCallbackInfo output)
	{
		output = default(OnMessageToServerCallbackInfo);
		output.Set(ref this);
	}
}
