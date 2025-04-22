using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DefinitionInternal : IGettable<Definition>, ISettable<Definition>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AchievementId;

	private IntPtr m_DisplayName;

	private IntPtr m_Description;

	private IntPtr m_LockedDisplayName;

	private IntPtr m_LockedDescription;

	private IntPtr m_HiddenDescription;

	private IntPtr m_CompletionDescription;

	private IntPtr m_UnlockedIconId;

	private IntPtr m_LockedIconId;

	private int m_IsHidden;

	private int m_StatThresholdsCount;

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

	public Utf8String DisplayName
	{
		get
		{
			Helper.Get(m_DisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public Utf8String Description
	{
		get
		{
			Helper.Get(m_Description, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Description);
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

	public Utf8String HiddenDescription
	{
		get
		{
			Helper.Get(m_HiddenDescription, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_HiddenDescription);
		}
	}

	public Utf8String CompletionDescription
	{
		get
		{
			Helper.Get(m_CompletionDescription, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CompletionDescription);
		}
	}

	public Utf8String UnlockedIconId
	{
		get
		{
			Helper.Get(m_UnlockedIconId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnlockedIconId);
		}
	}

	public Utf8String LockedIconId
	{
		get
		{
			Helper.Get(m_LockedIconId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LockedIconId);
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

	public void Set(ref Definition other)
	{
		m_ApiVersion = 1;
		AchievementId = other.AchievementId;
		DisplayName = other.DisplayName;
		Description = other.Description;
		LockedDisplayName = other.LockedDisplayName;
		LockedDescription = other.LockedDescription;
		HiddenDescription = other.HiddenDescription;
		CompletionDescription = other.CompletionDescription;
		UnlockedIconId = other.UnlockedIconId;
		LockedIconId = other.LockedIconId;
		IsHidden = other.IsHidden;
		StatThresholds = other.StatThresholds;
	}

	public void Set(ref Definition? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AchievementId = other.Value.AchievementId;
			DisplayName = other.Value.DisplayName;
			Description = other.Value.Description;
			LockedDisplayName = other.Value.LockedDisplayName;
			LockedDescription = other.Value.LockedDescription;
			HiddenDescription = other.Value.HiddenDescription;
			CompletionDescription = other.Value.CompletionDescription;
			UnlockedIconId = other.Value.UnlockedIconId;
			LockedIconId = other.Value.LockedIconId;
			IsHidden = other.Value.IsHidden;
			StatThresholds = other.Value.StatThresholds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AchievementId);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_Description);
		Helper.Dispose(ref m_LockedDisplayName);
		Helper.Dispose(ref m_LockedDescription);
		Helper.Dispose(ref m_HiddenDescription);
		Helper.Dispose(ref m_CompletionDescription);
		Helper.Dispose(ref m_UnlockedIconId);
		Helper.Dispose(ref m_LockedIconId);
		Helper.Dispose(ref m_StatThresholds);
	}

	public void Get(out Definition output)
	{
		output = default(Definition);
		output.Set(ref this);
	}
}
