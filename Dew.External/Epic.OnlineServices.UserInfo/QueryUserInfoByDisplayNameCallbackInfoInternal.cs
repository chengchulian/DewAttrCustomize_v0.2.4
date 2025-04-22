using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryUserInfoByDisplayNameCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryUserInfoByDisplayNameCallbackInfo>, ISettable<QueryUserInfoByDisplayNameCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private IntPtr m_DisplayName;

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

	public Utf8String DisplayName
	{
		get
		{
			Helper.Get(m_DisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public void Set(ref QueryUserInfoByDisplayNameCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		DisplayName = other.DisplayName;
	}

	public void Set(ref QueryUserInfoByDisplayNameCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			DisplayName = other.Value.DisplayName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_DisplayName);
	}

	public void Get(out QueryUserInfoByDisplayNameCallbackInfo output)
	{
		output = default(QueryUserInfoByDisplayNameCallbackInfo);
		output.Set(ref this);
	}
}
