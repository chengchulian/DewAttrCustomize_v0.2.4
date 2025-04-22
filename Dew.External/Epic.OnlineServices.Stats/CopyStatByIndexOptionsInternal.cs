using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyStatByIndexOptionsInternal : ISettable<CopyStatByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private uint m_StatIndex;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint StatIndex
	{
		set
		{
			m_StatIndex = value;
		}
	}

	public void Set(ref CopyStatByIndexOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		StatIndex = other.StatIndex;
	}

	public void Set(ref CopyStatByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			StatIndex = other.Value.StatIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
