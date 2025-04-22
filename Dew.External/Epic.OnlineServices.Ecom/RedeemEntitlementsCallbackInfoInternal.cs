using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RedeemEntitlementsCallbackInfoInternal : ICallbackInfoInternal, IGettable<RedeemEntitlementsCallbackInfo>, ISettable<RedeemEntitlementsCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private uint m_RedeemedEntitlementIdsCount;

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

	public uint RedeemedEntitlementIdsCount
	{
		get
		{
			return m_RedeemedEntitlementIdsCount;
		}
		set
		{
			m_RedeemedEntitlementIdsCount = value;
		}
	}

	public void Set(ref RedeemEntitlementsCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RedeemedEntitlementIdsCount = other.RedeemedEntitlementIdsCount;
	}

	public void Set(ref RedeemEntitlementsCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RedeemedEntitlementIdsCount = other.Value.RedeemedEntitlementIdsCount;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
	}

	public void Get(out RedeemEntitlementsCallbackInfo output)
	{
		output = default(RedeemEntitlementsCallbackInfo);
		output.Set(ref this);
	}
}
