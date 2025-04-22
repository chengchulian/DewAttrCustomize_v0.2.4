using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PinGrantInfoInternal : IGettable<PinGrantInfo>, ISettable<PinGrantInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserCode;

	private IntPtr m_VerificationURI;

	private int m_ExpiresIn;

	private IntPtr m_VerificationURIComplete;

	public Utf8String UserCode
	{
		get
		{
			Helper.Get(m_UserCode, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserCode);
		}
	}

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

	public int ExpiresIn
	{
		get
		{
			return m_ExpiresIn;
		}
		set
		{
			m_ExpiresIn = value;
		}
	}

	public Utf8String VerificationURIComplete
	{
		get
		{
			Helper.Get(m_VerificationURIComplete, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_VerificationURIComplete);
		}
	}

	public void Set(ref PinGrantInfo other)
	{
		m_ApiVersion = 2;
		UserCode = other.UserCode;
		VerificationURI = other.VerificationURI;
		ExpiresIn = other.ExpiresIn;
		VerificationURIComplete = other.VerificationURIComplete;
	}

	public void Set(ref PinGrantInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			UserCode = other.Value.UserCode;
			VerificationURI = other.Value.VerificationURI;
			ExpiresIn = other.Value.ExpiresIn;
			VerificationURIComplete = other.Value.VerificationURIComplete;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserCode);
		Helper.Dispose(ref m_VerificationURI);
		Helper.Dispose(ref m_VerificationURIComplete);
	}

	public void Get(out PinGrantInfo output)
	{
		output = default(PinGrantInfo);
		output.Set(ref this);
	}
}
