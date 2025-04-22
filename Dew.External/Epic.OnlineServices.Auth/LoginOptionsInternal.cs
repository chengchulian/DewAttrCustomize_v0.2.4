using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LoginOptionsInternal : ISettable<LoginOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Credentials;

	private AuthScopeFlags m_ScopeFlags;

	private LoginFlags m_LoginFlags;

	public Credentials? Credentials
	{
		set
		{
			Helper.Set<Credentials, CredentialsInternal>(ref value, ref m_Credentials);
		}
	}

	public AuthScopeFlags ScopeFlags
	{
		set
		{
			m_ScopeFlags = value;
		}
	}

	public LoginFlags LoginFlags
	{
		set
		{
			m_LoginFlags = value;
		}
	}

	public void Set(ref LoginOptions other)
	{
		m_ApiVersion = 3;
		Credentials = other.Credentials;
		ScopeFlags = other.ScopeFlags;
		LoginFlags = other.LoginFlags;
	}

	public void Set(ref LoginOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			Credentials = other.Value.Credentials;
			ScopeFlags = other.Value.ScopeFlags;
			LoginFlags = other.Value.LoginFlags;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Credentials);
	}
}
