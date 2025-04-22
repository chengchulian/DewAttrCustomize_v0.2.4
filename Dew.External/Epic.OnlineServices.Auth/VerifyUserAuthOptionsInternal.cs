using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct VerifyUserAuthOptionsInternal : ISettable<VerifyUserAuthOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AuthToken;

	public Token? AuthToken
	{
		set
		{
			Helper.Set<Token, TokenInternal>(ref value, ref m_AuthToken);
		}
	}

	public void Set(ref VerifyUserAuthOptions other)
	{
		m_ApiVersion = 1;
		AuthToken = other.AuthToken;
	}

	public void Set(ref VerifyUserAuthOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AuthToken = other.Value.AuthToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AuthToken);
	}
}
