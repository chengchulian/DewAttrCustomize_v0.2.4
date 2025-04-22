using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct StatInternal : IGettable<Stat>, ISettable<Stat>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Name;

	private long m_StartTime;

	private long m_EndTime;

	private int m_Value;

	public Utf8String Name
	{
		get
		{
			Helper.Get(m_Name, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Name);
		}
	}

	public DateTimeOffset? StartTime
	{
		get
		{
			Helper.Get(m_StartTime, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_StartTime);
		}
	}

	public DateTimeOffset? EndTime
	{
		get
		{
			Helper.Get(m_EndTime, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_EndTime);
		}
	}

	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			m_Value = value;
		}
	}

	public void Set(ref Stat other)
	{
		m_ApiVersion = 1;
		Name = other.Name;
		StartTime = other.StartTime;
		EndTime = other.EndTime;
		Value = other.Value;
	}

	public void Set(ref Stat? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Name = other.Value.Name;
			StartTime = other.Value.StartTime;
			EndTime = other.Value.EndTime;
			Value = other.Value.Value;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Name);
	}

	public void Get(out Stat output)
	{
		output = default(Stat);
		output.Set(ref this);
	}
}
