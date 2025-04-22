using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryOwnershipTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipTokenCallbackInfo>, ISettable<QueryOwnershipTokenCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_OwnershipToken;

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

	public Utf8String OwnershipToken
	{
		get
		{
			Helper.Get(m_OwnershipToken, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OwnershipToken);
		}
	}

	public void Set(ref QueryOwnershipTokenCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		OwnershipToken = other.OwnershipToken;
	}

	public void Set(ref QueryOwnershipTokenCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			OwnershipToken = other.Value.OwnershipToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_OwnershipToken);
	}

	public void Get(out QueryOwnershipTokenCallbackInfo output)
	{
		output = default(QueryOwnershipTokenCallbackInfo);
		output.Set(ref this);
	}
}
