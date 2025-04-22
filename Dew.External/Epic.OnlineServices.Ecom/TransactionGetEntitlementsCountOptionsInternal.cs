using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct TransactionGetEntitlementsCountOptionsInternal : ISettable<TransactionGetEntitlementsCountOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref TransactionGetEntitlementsCountOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref TransactionGetEntitlementsCountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
		}
	}

	public void Dispose()
	{
	}
}
