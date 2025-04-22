using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct EndPlayerSessionOptionsInternal : ISettable<EndPlayerSessionOptions>, IDisposable
{
	private int m_ApiVersion;

	private EndPlayerSessionOptionsAccountIdInternal m_AccountId;

	public EndPlayerSessionOptionsAccountId AccountId
	{
		set
		{
			Helper.Set(ref value, ref m_AccountId);
		}
	}

	public void Set(ref EndPlayerSessionOptions other)
	{
		m_ApiVersion = 1;
		AccountId = other.AccountId;
	}

	public void Set(ref EndPlayerSessionOptions? other)
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
