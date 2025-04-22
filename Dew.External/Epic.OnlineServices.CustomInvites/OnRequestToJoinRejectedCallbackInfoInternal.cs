using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnRequestToJoinRejectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRequestToJoinRejectedCallbackInfo>, ISettable<OnRequestToJoinRejectedCallbackInfo>, IDisposable
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

	public void Set(ref OnRequestToJoinRejectedCallbackInfo other)
	{
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref OnRequestToJoinRejectedCallbackInfo? other)
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

	public void Get(out OnRequestToJoinRejectedCallbackInfo output)
	{
		output = default(OnRequestToJoinRejectedCallbackInfo);
		output.Set(ref this);
	}
}
