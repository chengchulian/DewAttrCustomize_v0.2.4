using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryEntitlementTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryEntitlementTokenCallbackInfo>, ISettable<QueryEntitlementTokenCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_EntitlementToken;

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

	public Utf8String EntitlementToken
	{
		get
		{
			Helper.Get(m_EntitlementToken, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_EntitlementToken);
		}
	}

	public void Set(ref QueryEntitlementTokenCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		EntitlementToken = other.EntitlementToken;
	}

	public void Set(ref QueryEntitlementTokenCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			EntitlementToken = other.Value.EntitlementToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementToken);
	}

	public void Get(out QueryEntitlementTokenCallbackInfo output)
	{
		output = default(QueryEntitlementTokenCallbackInfo);
		output.Set(ref this);
	}
}
