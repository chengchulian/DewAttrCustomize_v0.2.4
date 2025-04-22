using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyTransactionByIdOptionsInternal : ISettable<CopyTransactionByIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_TransactionId;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String TransactionId
	{
		set
		{
			Helper.Set(value, ref m_TransactionId);
		}
	}

	public void Set(ref CopyTransactionByIdOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		TransactionId = other.TransactionId;
	}

	public void Set(ref CopyTransactionByIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			TransactionId = other.Value.TransactionId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TransactionId);
	}
}
