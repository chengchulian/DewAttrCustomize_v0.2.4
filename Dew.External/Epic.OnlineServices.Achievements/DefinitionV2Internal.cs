using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DefinitionV2Internal : IGettable<DefinitionV2>, ISettable<DefinitionV2>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AchievementId;

	private IntPtr m_UnlockedDisplayName;

	private IntPtr m_UnlockedDescription;

	private IntPtr m_LockedDisplayName;

	private IntPtr m_LockedDescription;

	private IntPtr m_FlavorText;

	private IntPtr m_UnlockedIconURL;

	private IntPtr m_LockedIconURL;

	private int m_IsHidden;

	private uint m_StatThresholdsCount;

	private IntPtr m_StatThresholds;

	public Utf8String AchievementId
	{
		get
		{
			Helper.Get(m_AchievementId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AchievementId);
		}
	}

	public Utf8String UnlockedDisplayName
	{
		get
		{
			Helper.Get(m_UnlockedDisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnlockedDisplayName);
		}
	}

	public Utf8String UnlockedDescription
	{
		get
		{
			Helper.Get(m_UnlockedDescription, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnlockedDescription);
		}
	}

	public Utf8String LockedDisplayName
	{
		get
		{
			Helper.Get(m_LockedDisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LockedDisplayName);
		}
	}

	public Utf8String LockedDescription
	{
		get
		{
			Helper.Get(m_LockedDescription, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LockedDescription);
		}
	}

	public Utf8String FlavorText
	{
		get
		{
			Helper.Get(m_FlavorText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_FlavorText);
		}
	}

	public Utf8String UnlockedIconURL
	{
		get
		{
			Helper.Get(m_UnlockedIconURL, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnlockedIconURL);
		}
	}

	public Utf8String LockedIconURL
	{
		get
		{
			Helper.Get(m_LockedIconURL, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LockedIconURL);
		}
	}

	public bool IsHidden
	{
		get
		{
			Helper.Get(m_IsHidden, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsHidden);
		}
	}

	public StatThresholds[] StatThresholds
	{
		get
		{
			Helper.Get<StatThresholdsInternal, StatThresholds>(m_StatThresholds, out var value, m_StatThresholdsCount);
			return value;
		}
		set
		{
			Helper.Set<StatThresholds, StatThresholdsInternal>(ref value, ref m_StatThresholds, out m_StatThresholdsCount);
		}
	}

	public void Set(ref DefinitionV2 other)
	{
		m_ApiVersion = 2;
		AchievementId = other.AchievementId;
		UnlockedDisplayName = other.UnlockedDisplayName;
		UnlockedDescription = other.UnlockedDescription;
		LockedDisplayName = other.LockedDisplayName;
		LockedDescription = other.LockedDescription;
		FlavorText = other.FlavorText;
		UnlockedIconURL = other.UnlockedIconURL;
		LockedIconURL = other.LockedIconURL;
		IsHidden = other.IsHidden;
		StatThresholds = other.StatThresholds;
	}

	public void Set(ref DefinitionV2? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			AchievementId = other.Value.AchievementId;
			UnlockedDisplayName = other.Value.UnlockedDisplayName;
			UnlockedDescription = other.Value.UnlockedDescription;
			LockedDisplayName = other.Value.LockedDisplayName;
			LockedDescription = other.Value.LockedDescription;
			FlavorText = other.Value.FlavorText;
			UnlockedIconURL = other.Value.UnlockedIconURL;
			LockedIconURL = other.Value.LockedIconURL;
			IsHidden = other.Value.IsHidden;
			StatThresholds = other.Value.StatThresholds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AchievementId);
		Helper.Dispose(ref m_UnlockedDisplayName);
		Helper.Dispose(ref m_UnlockedDescription);
		Helper.Dispose(ref m_LockedDisplayName);
		Helper.Dispose(ref m_LockedDescription);
		Helper.Dispose(ref m_FlavorText);
		Helper.Dispose(ref m_UnlockedIconURL);
		Helper.Dispose(ref m_LockedIconURL);
		Helper.Dispose(ref m_StatThresholds);
	}

	public void Get(out DefinitionV2 output)
	{
		output = default(DefinitionV2);
		output.Set(ref this);
	}
}
