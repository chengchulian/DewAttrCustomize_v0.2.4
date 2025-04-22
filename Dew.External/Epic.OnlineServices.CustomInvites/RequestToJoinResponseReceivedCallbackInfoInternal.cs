using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RequestToJoinResponseReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestToJoinResponseReceivedCallbackInfo>, ISettable<RequestToJoinResponseReceivedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_FromUserId;

	private IntPtr m_ToUserId;

	private RequestToJoinResponse m_Response;

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

	public RequestToJoinResponse Response
	{
		get
		{
			return m_Response;
		}
		set
		{
			m_Response = value;
		}
	}

	public void Set(ref RequestToJoinResponseReceivedCallbackInfo other)
	{
		ClientData = other.ClientData;
		FromUserId = other.FromUserId;
		ToUserId = other.ToUserId;
		Response = other.Response;
	}

	public void Set(ref RequestToJoinResponseReceivedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			FromUserId = other.Value.FromUserId;
			ToUserId = other.Value.ToUserId;
			Response = other.Value.Response;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_FromUserId);
		Helper.Dispose(ref m_ToUserId);
	}

	public void Get(out RequestToJoinResponseReceivedCallbackInfo output)
	{
		output = default(RequestToJoinResponseReceivedCallbackInfo);
		output.Set(ref this);
	}
}
