using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LoginCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginCallbackInfo>, ISettable<LoginCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_PinGrantInfo;

	private IntPtr m_ContinuanceToken;

	private IntPtr m_AccountFeatureRestrictedInfo_DEPRECATED;

	private IntPtr m_SelectedAccountId;

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

	public PinGrantInfo? PinGrantInfo
	{
		get
		{
			Helper.Get<PinGrantInfoInternal, PinGrantInfo>(m_PinGrantInfo, out PinGrantInfo? value);
			return value;
		}
		set
		{
			Helper.Set<PinGrantInfo, PinGrantInfoInternal>(ref value, ref m_PinGrantInfo);
		}
	}

	public ContinuanceToken ContinuanceToken
	{
		get
		{
			Helper.Get(m_ContinuanceToken, out ContinuanceToken value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ContinuanceToken);
		}
	}

	public AccountFeatureRestrictedInfo? AccountFeatureRestrictedInfo_DEPRECATED
	{
		get
		{
			Helper.Get<AccountFeatureRestrictedInfoInternal, AccountFeatureRestrictedInfo>(m_AccountFeatureRestrictedInfo_DEPRECATED, out AccountFeatureRestrictedInfo? value);
			return value;
		}
		set
		{
			Helper.Set<AccountFeatureRestrictedInfo, AccountFeatureRestrictedInfoInternal>(ref value, ref m_AccountFeatureRestrictedInfo_DEPRECATED);
		}
	}

	public EpicAccountId SelectedAccountId
	{
		get
		{
			Helper.Get(m_SelectedAccountId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SelectedAccountId);
		}
	}

	public void Set(ref LoginCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PinGrantInfo = other.PinGrantInfo;
		ContinuanceToken = other.ContinuanceToken;
		AccountFeatureRestrictedInfo_DEPRECATED = other.AccountFeatureRestrictedInfo_DEPRECATED;
		SelectedAccountId = other.SelectedAccountId;
	}

	public void Set(ref LoginCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			PinGrantInfo = other.Value.PinGrantInfo;
			ContinuanceToken = other.Value.ContinuanceToken;
			AccountFeatureRestrictedInfo_DEPRECATED = other.Value.AccountFeatureRestrictedInfo_DEPRECATED;
			SelectedAccountId = other.Value.SelectedAccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_PinGrantInfo);
		Helper.Dispose(ref m_ContinuanceToken);
		Helper.Dispose(ref m_AccountFeatureRestrictedInfo_DEPRECATED);
		Helper.Dispose(ref m_SelectedAccountId);
	}

	public void Get(out LoginCallbackInfo output)
	{
		output = default(LoginCallbackInfo);
		output.Set(ref this);
	}
}
