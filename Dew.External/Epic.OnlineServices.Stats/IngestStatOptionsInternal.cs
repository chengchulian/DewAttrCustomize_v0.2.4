using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct IngestStatOptionsInternal : ISettable<IngestStatOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_Stats;

	private uint m_StatsCount;

	private IntPtr m_TargetUserId;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public IngestData[] Stats
	{
		set
		{
			Helper.Set<IngestData, IngestDataInternal>(ref value, ref m_Stats, out m_StatsCount);
		}
	}

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref IngestStatOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		Stats = other.Stats;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref IngestStatOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			Stats = other.Value.Stats;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Stats);
		Helper.Dispose(ref m_TargetUserId);
	}
}
