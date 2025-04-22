using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RequestToJoinReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestToJoinReceivedCallbackInfo>, ISettable<RequestToJoinReceivedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_FromUserId;

	private IntPtr m_ToUserId;

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

	public ProductUserId FromUserId
	{
		get
		{
			Helper.Get(m_FromUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_FromUserId);
		}
	}

	public ProductUserId ToUserId
	{
		get
		{
			Helper.Get(m_ToUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ToUserId);
		}
	}

	public void Set(ref RequestToJoinReceivedCallbackInfo other)
	{
		ClientData = other.ClientData;
		FromUserId = other.FromUserId;
		ToUserId = other.ToUserId;
	}

	public void Set(ref RequestToJoinReceivedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			FromUserId = other.Value.FromUserId;
			ToUserId = other.Value.ToUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_FromUserId);
		Helper.Dispose(ref m_ToUserId);
	}

	public void Get(out RequestToJoinReceivedCallbackInfo output)
	{
		output = default(RequestToJoinReceivedCallbackInfo);
		output.Set(ref this);
	}
}
