using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryStatsOptionsInternal : ISettable<QueryStatsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private long m_StartTime;

	private long m_EndTime;

	private IntPtr m_StatNames;

	private uint m_StatNamesCount;

	private IntPtr m_TargetUserId;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public DateTimeOffset? StartTime
	{
		set
		{
			Helper.Set(value, ref m_StartTime);
		}
	}

	public DateTimeOffset? EndTime
	{
		set
		{
			Helper.Set(value, ref m_EndTime);
		}
	}

	public Utf8String[] StatNames
	{
		set
		{
			Helper.Set(value, ref m_StatNames, isArrayItemAllocated: true, out m_StatNamesCount);
		}
	}

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref QueryStatsOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		StartTime = other.StartTime;
		EndTime = other.EndTime;
		StatNames = other.StatNames;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref QueryStatsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			StartTime = other.Value.StartTime;
			EndTime = other.Value.EndTime;
			StatNames = other.Value.StatNames;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_StatNames);
		Helper.Dispose(ref m_TargetUserId);
	}
}
