using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryUserInfoByExternalAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryUserInfoByExternalAccountCallbackInfo>, ISettable<QueryUserInfoByExternalAccountCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_ExternalAccountId;

	private ExternalAccountType m_AccountType;

	private IntPtr m_TargetUserId;

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

	public Utf8String ExternalAccountId
	{
		get
		{
			Helper.Get(m_ExternalAccountId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ExternalAccountId);
		}
	}

	public ExternalAccountType AccountType
	{
		get
		{
			return m_AccountType;
		}
		set
		{
			m_AccountType = value;
		}
	}

	public EpicAccountId TargetUserId
	{
		get
		{
			Helper.Get(m_TargetUserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref QueryUserInfoByExternalAccountCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		ExternalAccountId = other.ExternalAccountId;
		AccountType = other.AccountType;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref QueryUserInfoByExternalAccountCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			ExternalAccountId = other.Value.ExternalAccountId;
			AccountType = other.Value.AccountType;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ExternalAccountId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out QueryUserInfoByExternalAccountCallbackInfo output)
	{
		output = default(QueryUserInfoByExternalAccountCallbackInfo);
		output.Set(ref this);
	}
}
