using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnAchievementsUnlockedCallbackV2InfoInternal : ICallbackInfoInternal, IGettable<OnAchievementsUnlockedCallbackV2Info>, ISettable<OnAchievementsUnlockedCallbackV2Info>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_UserId;

	private IntPtr m_AchievementId;

	private long m_UnlockTime;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public ProductUserId UserId
	{
		get
		{
			Helper.Get(m_UserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

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

	public void Set(ref OnAchievementsUnlockedCallbackV2Info other)
	{
		ClientData = other.ClientData;
		UserId = other.UserId;
		AchievementId = other.AchievementId;
		UnlockTime = other.UnlockTime;
	}

	public void Set(ref OnAchievementsUnlockedCallbackV2Info? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			UserId = other.Value.UserId;
			AchievementId = other.Value.AchievementId;
			UnlockTime = other.Value.UnlockTime;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_AchievementId);
	}

	public void Get(out OnAchievementsUnlockedCallbackV2Info output)
	{
		output = default(OnAchievementsUnlockedCallbackV2Info);
		output.Set(ref this);
	}
}
