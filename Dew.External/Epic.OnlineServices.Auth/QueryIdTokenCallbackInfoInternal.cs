using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryIdTokenCallbackInfo>, ISettable<QueryIdTokenCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetAccountId;

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

	public EpicAccountId TargetAccountId
	{
		get
		{
			Helper.Get(m_TargetAccountId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetAccountId);
		}
	}

	public void Set(ref QueryIdTokenCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetAccountId = other.TargetAccountId;
	}

	public void Set(ref QueryIdTokenCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			TargetAccountId = other.Value.TargetAccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetAccountId);
	}

	public void Get(out QueryIdTokenCallbackInfo output)
	{
		output = default(QueryIdTokenCallbackInfo);
		output.Set(ref this);
	}
}
