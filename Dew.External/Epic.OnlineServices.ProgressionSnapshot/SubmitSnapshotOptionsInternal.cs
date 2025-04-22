using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SubmitSnapshotOptionsInternal : ISettable<SubmitSnapshotOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_SnapshotId;

	public uint SnapshotId
	{
		set
		{
			m_SnapshotId = value;
		}
	}

	public void Set(ref SubmitSnapshotOptions other)
	{
		m_ApiVersion = 1;
		SnapshotId = other.SnapshotId;
	}

	public void Set(ref SubmitSnapshotOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SnapshotId = other.Value.SnapshotId;
		}
	}

	public void Dispose()
	{
	}
}
