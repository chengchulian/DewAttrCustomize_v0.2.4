using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PlayerStatInfoInternal : IGettable<PlayerStatInfo>, ISettable<PlayerStatInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Name;

	private int m_CurrentValue;

	private int m_ThresholdValue;

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

	public int CurrentValue
	{
		get
		{
			return m_CurrentValue;
		}
		set
		{
			m_CurrentValue = value;
		}
	}

	public int ThresholdValue
	{
		get
		{
			return m_ThresholdValue;
		}
		set
		{
			m_ThresholdValue = value;
		}
	}

	public void Set(ref PlayerStatInfo other)
	{
		m_ApiVersion = 1;
		Name = other.Name;
		CurrentValue = other.CurrentValue;
		ThresholdValue = other.ThresholdValue;
	}

	public void Set(ref PlayerStatInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Name = other.Value.Name;
			CurrentValue = other.Value.CurrentValue;
			ThresholdValue = other.Value.ThresholdValue;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Name);
	}

	public void Get(out PlayerStatInfo output)
	{
		output = default(PlayerStatInfo);
		output.Set(ref this);
	}
}
