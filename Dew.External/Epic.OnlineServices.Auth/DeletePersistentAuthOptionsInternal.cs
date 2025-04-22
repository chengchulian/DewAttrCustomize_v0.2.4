using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DeletePersistentAuthOptionsInternal : ISettable<DeletePersistentAuthOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_RefreshToken;

	public Utf8String RefreshToken
	{
		set
		{
			Helper.Set(value, ref m_RefreshToken);
		}
	}

	public void Set(ref DeletePersistentAuthOptions other)
	{
		m_ApiVersion = 2;
		RefreshToken = other.RefreshToken;
	}

	public void Set(ref DeletePersistentAuthOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			RefreshToken = other.Value.RefreshToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_RefreshToken);
	}
}
