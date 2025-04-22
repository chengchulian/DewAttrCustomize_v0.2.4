using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct StatThresholdsInternal : IGettable<StatThresholds>, ISettable<StatThresholds>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Name;

	private int m_Threshold;

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

	public int Threshold
	{
		get
		{
			return m_Threshold;
		}
		set
		{
			m_Threshold = value;
		}
	}

	public void Set(ref StatThresholds other)
	{
		m_ApiVersion = 1;
		Name = other.Name;
		Threshold = other.Threshold;
	}

	public void Set(ref StatThresholds? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Name = other.Value.Name;
			Threshold = other.Value.Threshold;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Name);
	}

	public void Get(out StatThresholds output)
	{
		output = default(StatThresholds);
		output.Set(ref this);
	}
}
