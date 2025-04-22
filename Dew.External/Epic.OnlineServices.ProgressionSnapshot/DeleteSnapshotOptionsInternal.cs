using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DeleteSnapshotOptionsInternal : ISettable<DeleteSnapshotOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref DeleteSnapshotOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref DeleteSnapshotOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
