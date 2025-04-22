using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct JoinSessionAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinSessionAcceptedCallbackInfo>, ISettable<JoinSessionAcceptedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private ulong m_UiEventId;

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

	public ulong UiEventId
	{
		get
		{
			return m_UiEventId;
		}
		set
		{
			m_UiEventId = value;
		}
	}

	public void Set(ref JoinSessionAcceptedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		UiEventId = other.UiEventId;
	}

	public void Set(ref JoinSessionAcceptedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			UiEventId = other.Value.UiEventId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
	}

	public void Get(out JoinSessionAcceptedCallbackInfo output)
	{
		output = default(JoinSessionAcceptedCallbackInfo);
		output.Set(ref this);
	}
}
