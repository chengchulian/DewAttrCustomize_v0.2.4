using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PresenceChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<PresenceChangedCallbackInfo>, ISettable<PresenceChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_PresenceUserId;

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

	public EpicAccountId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public EpicAccountId PresenceUserId
	{
		get
		{
			Helper.Get(m_PresenceUserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_PresenceUserId);
		}
	}

	public void Set(ref PresenceChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PresenceUserId = other.PresenceUserId;
	}

	public void Set(ref PresenceChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			PresenceUserId = other.Value.PresenceUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_PresenceUserId);
	}

	public void Get(out PresenceChangedCallbackInfo output)
	{
		output = default(PresenceChangedCallbackInfo);
		output.Set(ref this);
	}
}
