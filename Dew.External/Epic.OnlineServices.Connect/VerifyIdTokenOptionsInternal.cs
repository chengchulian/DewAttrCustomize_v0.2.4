using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct VerifyIdTokenOptionsInternal : ISettable<VerifyIdTokenOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_IdToken;

	public IdToken? IdToken
	{
		set
		{
			Helper.Set<IdToken, IdTokenInternal>(ref value, ref m_IdToken);
		}
	}

	public void Set(ref VerifyIdTokenOptions other)
	{
		m_ApiVersion = 1;
		IdToken = other.IdToken;
	}

	public void Set(ref VerifyIdTokenOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			IdToken = other.Value.IdToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_IdToken);
	}
}
