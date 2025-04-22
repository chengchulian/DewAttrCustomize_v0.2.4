using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddProgressionOptionsInternal : ISettable<AddProgressionOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_SnapshotId;

	private IntPtr m_Key;

	private IntPtr m_Value;

	public uint SnapshotId
	{
		set
		{
			m_SnapshotId = value;
		}
	}

	public Utf8String Key
	{
		set
		{
			Helper.Set(value, ref m_Key);
		}
	}

	public Utf8String Value
	{
		set
		{
			Helper.Set(value, ref m_Value);
		}
	}

	public void Set(ref AddProgressionOptions other)
	{
		m_ApiVersion = 1;
		SnapshotId = other.SnapshotId;
		Key = other.Key;
		Value = other.Value;
	}

	public void Set(ref AddProgressionOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SnapshotId = other.Value.SnapshotId;
			Key = other.Value.Key;
			Value = other.Value.Value;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Key);
		Helper.Dispose(ref m_Value);
	}
}
