using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyIdTokenOptionsInternal : ISettable<CopyIdTokenOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AccountId;

	public EpicAccountId AccountId
	{
		set
		{
			Helper.Set(value, ref m_AccountId);
		}
	}

	public void Set(ref CopyIdTokenOptions other)
	{
		m_ApiVersion = 1;
		AccountId = other.AccountId;
	}

	public void Set(ref CopyIdTokenOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AccountId = other.Value.AccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AccountId);
	}
}
