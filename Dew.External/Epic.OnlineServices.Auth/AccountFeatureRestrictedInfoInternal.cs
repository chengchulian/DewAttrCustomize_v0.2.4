using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AccountFeatureRestrictedInfoInternal : IGettable<AccountFeatureRestrictedInfo>, ISettable<AccountFeatureRestrictedInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_VerificationURI;

	public Utf8String VerificationURI
	{
		get
		{
			Helper.Get(m_VerificationURI, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_VerificationURI);
		}
	}

	public void Set(ref AccountFeatureRestrictedInfo other)
	{
		m_ApiVersion = 1;
		VerificationURI = other.VerificationURI;
	}

	public void Set(ref AccountFeatureRestrictedInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			VerificationURI = other.Value.VerificationURI;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_VerificationURI);
	}

	public void Get(out AccountFeatureRestrictedInfo output)
	{
		output = default(AccountFeatureRestrictedInfo);
		output.Set(ref this);
	}
}
