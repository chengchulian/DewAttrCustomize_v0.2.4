using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryAgeGateCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryAgeGateCallbackInfo>, ISettable<QueryAgeGateCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_CountryCode;

	private uint m_AgeOfConsent;

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

	public Utf8String CountryCode
	{
		get
		{
			Helper.Get(m_CountryCode, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CountryCode);
		}
	}

	public uint AgeOfConsent
	{
		get
		{
			return m_AgeOfConsent;
		}
		set
		{
			m_AgeOfConsent = value;
		}
	}

	public void Set(ref QueryAgeGateCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		CountryCode = other.CountryCode;
		AgeOfConsent = other.AgeOfConsent;
	}

	public void Set(ref QueryAgeGateCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			CountryCode = other.Value.CountryCode;
			AgeOfConsent = other.Value.AgeOfConsent;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_CountryCode);
	}

	public void Get(out QueryAgeGateCallbackInfo output)
	{
		output = default(QueryAgeGateCallbackInfo);
		output.Set(ref this);
	}
}
