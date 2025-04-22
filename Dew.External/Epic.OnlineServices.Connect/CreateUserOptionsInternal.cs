using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CreateUserOptionsInternal : ISettable<CreateUserOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ContinuanceToken;

	public ContinuanceToken ContinuanceToken
	{
		set
		{
			Helper.Set(value, ref m_ContinuanceToken);
		}
	}

	public void Set(ref CreateUserOptions other)
	{
		m_ApiVersion = 1;
		ContinuanceToken = other.ContinuanceToken;
	}

	public void Set(ref CreateUserOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ContinuanceToken = other.Value.ContinuanceToken;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ContinuanceToken);
	}
}
