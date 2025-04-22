using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnRequestToJoinAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRequestToJoinAcceptedCallbackInfo>, ISettable<OnRequestToJoinAcceptedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_TargetUserId;

	private IntPtr m_LocalUserId;

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

	public ProductUserId TargetUserId
	{
		get
		{
			Helper.Get(m_TargetUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetUserId);
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

	public void Set(ref OnRequestToJoinAcceptedCallbackInfo other)
	{
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref OnRequestToJoinAcceptedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			TargetUserId = other.Value.TargetUserId;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_LocalUserId);
	}

	public void Get(out OnRequestToJoinAcceptedCallbackInfo output)
	{
		output = default(OnRequestToJoinAcceptedCallbackInfo);
		output.Set(ref this);
	}
}
