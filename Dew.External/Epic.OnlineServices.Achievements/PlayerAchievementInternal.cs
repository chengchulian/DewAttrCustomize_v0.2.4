using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PlayerAchievementInternal : IGettable<PlayerAchievement>, ISettable<PlayerAchievement>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AchievementId;

	private double m_Progress;

	private long m_UnlockTime;

	private int m_StatInfoCount;

	private IntPtr m_StatInfo;

	private IntPtr m_DisplayName;

	private IntPtr m_Description;

	private IntPtr m_IconURL;

	private IntPtr m_FlavorText;

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

	public double Progress
	{
		get
		{
			return m_Progress;
		}
		set
		{
			m_Progress = value;
		}
	}

	public DateTimeOffset? UnlockTime
	{
		get
		{
			Helper.Get(m_UnlockTime, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnlockTime);
		}
	}

	public PlayerStatInfo[] StatInfo
	{
		get
		{
			Helper.Get<PlayerStatInfoInternal, PlayerStatInfo>(m_StatInfo, out var value, m_StatInfoCount);
			return value;
		}
		set
		{
			Helper.Set<PlayerStatInfo, PlayerStatInfoInternal>(ref value, ref m_StatInfo, out m_StatInfoCount);
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

	public Utf8String IconURL
	{
		get
		{
			Helper.Get(m_IconURL, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IconURL);
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

	public void Set(ref PlayerAchievement other)
	{
		m_ApiVersion = 2;
		AchievementId = other.AchievementId;
		Progress = other.Progress;
		UnlockTime = other.UnlockTime;
		StatInfo = other.StatInfo;
		DisplayName = other.DisplayName;
		Description = other.Description;
		IconURL = other.IconURL;
		FlavorText = other.FlavorText;
	}

	public void Set(ref PlayerAchievement? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			AchievementId = other.Value.AchievementId;
			Progress = other.Value.Progress;
			UnlockTime = other.Value.UnlockTime;
			StatInfo = other.Value.StatInfo;
			DisplayName = other.Value.DisplayName;
			Description = other.Value.Description;
			IconURL = other.Value.IconURL;
			FlavorText = other.Value.FlavorText;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AchievementId);
		Helper.Dispose(ref m_StatInfo);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_Description);
		Helper.Dispose(ref m_IconURL);
		Helper.Dispose(ref m_FlavorText);
	}

	public void Get(out PlayerAchievement output)
	{
		output = default(PlayerAchievement);
		output.Set(ref this);
	}
}
