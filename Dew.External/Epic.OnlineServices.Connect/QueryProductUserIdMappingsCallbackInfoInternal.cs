using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryProductUserIdMappingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryProductUserIdMappingsCallbackInfo>, ISettable<QueryProductUserIdMappingsCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	public Result ResultCode
	{
		get
		{
			return m_ResultCode;
		}
		set
		{
			m_ResultCode = value;
		}
	}

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

	public void Set(ref QueryProductUserIdMappingsCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref QueryProductUserIdMappingsCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
	}

	public void Get(out QueryProductUserIdMappingsCallbackInfo output)
	{
		output = default(QueryProductUserIdMappingsCallbackInfo);
		output.Set(ref this);
	}
}
